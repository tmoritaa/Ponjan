using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Game {
    private Dictionary<PhaseNode.PhaseID, PhaseNode> phaseNodes = new Dictionary<PhaseNode.PhaseID, PhaseNode>();
    private PhaseNode curPhase;
    private IEnumerator curPhaseEnumerator;

    private IEnumerator mainGameLoop;

    private List<HandCombination> handCombinations = new List<HandCombination>();

    private List<Player> players = new List<Player>();
    public List<Player> Players {
        get { return this.players; }
    }

    private Decision decisionWaitingResponse = null;
    private Queue<Decision> decisionsToProcess = new Queue<Decision>();

    private Queue<Command> commandsToProcess = new Queue<Command>();

    private Queue<UIResponseRequest> uiResponseRequests = new Queue<UIResponseRequest>();
    private Queue<UIUpdateRequest> uiUpdateRequests = new Queue<UIUpdateRequest>();

    private DeckZone deck = new DeckZone();
    public DeckZone Deck {
        get { return this.deck; }
    }

    private int numberOfRounds = 0;
    public int NumberOfRounds {
        get { return this.numberOfRounds; }
    }

    private int curRound = 0;
    public int CurRound {
        get { return this.curRound; }
    }

    private bool gameComplete = false;
    public bool GameComplete {
        set { this.gameComplete = value; }
    }

    private PhaseNode nextPhaseToForce = null;

    public static List<int> CalculateScoreForNoDeck(List<Player> players) {
        List<int> scoreChangePerPlayer = new List<int>() { 0, 0, 0 };

        List<Player> playersTenpai = new List<Player>();
        List<Player> playersNoTenpai = new List<Player>();
        foreach (Player player in players) {
            if (player.OneAwayFromCompletion()) {
                playersTenpai.Add(player);
            } else {
                playersNoTenpai.Add(player);
            }
        }

        if (playersTenpai.Count == 0 || playersNoTenpai.Count == 0) {
            return scoreChangePerPlayer;
        }

        const int BASE_POINTS = 30;
        foreach (Player player in players) {
            if (playersTenpai.Exists(p => p == player)) {
                scoreChangePerPlayer[player.Id] = BASE_POINTS / playersNoTenpai.Count;
            } else {
                scoreChangePerPlayer[player.Id] = -BASE_POINTS / playersNoTenpai.Count;
            } 
        }

        return scoreChangePerPlayer;
    }

    public static int CalculateScoreFromCombinations(List<HandCombination> handCombs, bool isBoss) {
        int points = 0;
        int finalScore = 0;
        handCombs.ForEach(h => points += h.Score);

        if (points >= 4) {
            points -= 4;
            finalScore += 4 * (((int)(points / 2)) + 1) + 4;
        } else {
            finalScore += (int)Math.Pow(2, points - 1);
        }

        int multiplier = (isBoss) ? 15 : 10;

        return finalScore * multiplier;
    }

    public void EnqueueDecision(Decision decision) {
        this.decisionsToProcess.Enqueue(decision);
    }

    public void EnqueueCommand(Command command) {
        this.commandsToProcess.Enqueue(command);
    }

    public void EnqueueUIResponseRequest(UIResponseRequest request) {
        this.uiResponseRequests.Enqueue(request);
    }

    public void EnqueueUIUpdateRequest(UIUpdateRequest request) {
        this.uiUpdateRequests.Enqueue(request);
    }

    public bool HandleUIResponse(object response) {
        return this.decisionWaitingResponse.HandleUIResponse(response);
    }

    public void ForcePhaseAsNext(PhaseNode.PhaseID phaseId) {
        this.nextPhaseToForce = this.phaseNodes[phaseId];
    }

    private bool CheckIfShouldYield() {
        return this.uiResponseRequests.Count > 0 || this.uiUpdateRequests.Count > 0;
    }

    public void ContinueMainGameLoop() {
        this.mainGameLoop.MoveNext();

        // TODO: this will have to be moved somewhere else, or at least not run for every game instance, since this will break when performing AI.
        CombatSceneController.Instance.RequestUpdates(this.uiUpdateRequests);
        CombatSceneController.Instance.RequestResponses(this.uiResponseRequests);
    }

    public void Initialize(int numRounds, Player.PlayerType[] playerTypes, string[] names, List<TileSetupData.TileSetupEntry> tileSetupEntries) {
        this.numberOfRounds = numRounds;
        this.curRound = 0;

        for (int i = 0; i < playerTypes.Length; ++i) {
            Player player = new Player(playerTypes[i], i, names[i]);
            this.players.Add(player);
        }

        foreach(TileSetupData.TileSetupEntry setupEntry in tileSetupEntries) {
            for(int i = 0; i < setupEntry.numberInDeck; ++i) {
                Tile tile = new Tile(setupEntry.type, setupEntry.id);
                this.deck.AddTile(tile);
            }
        }

        PhaseNode gameStartPhase = new GameStartPhase();
        PhaseNode drawPhase = new DrawPhase();
        PhaseNode completeHandPhase = new CompleteHandPhase();
        PhaseNode discardPhase = new DiscardPhase();
        PhaseNode stealPhase = new StealPhase();
        PhaseNode changeActivePlayerPhase = new ChangeActivePlayerPhase();

        gameStartPhase.Next = drawPhase;
        drawPhase.Next = completeHandPhase;
        completeHandPhase.Next = discardPhase;
        discardPhase.Next = stealPhase;
        stealPhase.Next = changeActivePlayerPhase;
        changeActivePlayerPhase.Next = drawPhase;

        this.phaseNodes[gameStartPhase.PhaseId] = gameStartPhase;
        this.phaseNodes[drawPhase.PhaseId] = drawPhase;
        this.phaseNodes[completeHandPhase.PhaseId] = completeHandPhase;
        this.phaseNodes[discardPhase.PhaseId] = discardPhase;
        this.phaseNodes[stealPhase.PhaseId] = stealPhase;
        this.phaseNodes[changeActivePlayerPhase.PhaseId] = changeActivePlayerPhase;

        this.handCombinations.Add(new AllSameColorCombination());
        this.handCombinations.Add(new AllSameCombination());
        this.handCombinations.Add(new AllSameIdCombination());
        this.handCombinations.Add(new OneColorAndWhitesCombination());
        this.handCombinations.Add(new ThreeColorCombination());
        this.handCombinations.Add(new TwoIdenticalCombination());
        this.handCombinations.Add(new ReachHandCombination());
        this.handCombinations.Add(new NoStealsCombination());
        this.handCombinations.Add(new CompleteWithDrawCombination());
        this.handCombinations.Add(new IppatsuCombination());
        foreach (TileSetupData.TileSetupEntry entry in tileSetupEntries) {
            if (entry.type == Tile.TileType.Dragon) {
                this.handCombinations.Add(new DragonCombination(entry.id));
            }
        }
    }

    public void Reset() {
        this.players.ForEach(p => p.Reset(this));
        this.commandsToProcess.Clear();
        this.decisionsToProcess.Clear();
        this.uiResponseRequests.Clear();
        this.uiUpdateRequests.Clear();
        this.decisionWaitingResponse = null;
        this.gameComplete = false;
        this.Deck.ResetTiles();

        this.EnqueueUIUpdateRequest(new UIUpdateRequest(UIUpdateRequest.UpdateType.Reset));
    }

    public void StartNewRound() {
        this.curPhase = this.phaseNodes[PhaseNode.PhaseID.StartGame];
        this.curPhaseEnumerator = this.curPhase.PerformPhase(this);

        this.curRound += 1;

        this.mainGameLoop = this.Resume();
        this.ContinueMainGameLoop();
    }

    public List<HandCombination> ReturnValidCombinations(List<Tile> tiles, HandCombination.CompletionType compType) {
        List<HandCombination> validCombs = new List<HandCombination>();

        foreach (HandCombination comb in this.handCombinations) {
            bool valid = comb.HandHasCombination(tiles, compType);
            if (valid) {
                validCombs.Add(comb);
            }
        }

        return validCombs;
    }

    public IEnumerator Resume() {
        while (!this.gameComplete) {
            bool phaseStillProc = this.curPhaseEnumerator.MoveNext();

            // Let frontend process all game requests.
            if (this.CheckIfShouldYield()) {
                yield return 0;
            }

            // Process all decisions.
            {
                IEnumerator decisionProcEnumerator = this.ProcessQueuedDecisions();
                bool hasNext = true;
                do {
                    hasNext = decisionProcEnumerator.MoveNext();
                    yield return 0;
                } while (hasNext);
            }

            {
                IEnumerator commandProcEnumerator = this.ProcessQueuedCommands();
                bool hasNext = true;
                do {
                    hasNext = commandProcEnumerator.MoveNext();
                    yield return 0;
                } while (hasNext);
            }

            // Let frontend process all game requests.
            if (this.CheckIfShouldYield()) {
                yield return 0;
            }

            if (!phaseStillProc) {
                if (this.nextPhaseToForce != null) {
                    this.curPhase = this.nextPhaseToForce;
                    this.nextPhaseToForce = null;
                } else {
                    this.curPhase = this.curPhase.Next;
                }
                
                this.curPhaseEnumerator = this.curPhase.PerformPhase(this);
                this.EnqueueUIUpdateRequest(new UIUpdateRequest(UIUpdateRequest.UpdateType.UpdateBoard));
                yield return 0;
            }
        }

        this.Reset();

        yield break;
    }

    private IEnumerator ProcessQueuedDecisions() {
        while (this.decisionsToProcess.Count > 0) {
            Decision decision = this.decisionsToProcess.Dequeue();
            this.decisionWaitingResponse = decision;

            IEnumerator decEnum = decision.PerformDecision();

            bool decStillProc = true;
            do {
                decStillProc = decEnum.MoveNext();
                yield return 0;
            } while (decStillProc);
        }

        yield break;
    }

    private IEnumerator ProcessQueuedCommands() {
        while (this.commandsToProcess.Count > 0) {
            Command command = this.commandsToProcess.Dequeue();

            IEnumerator cmdEnum = command.PerformCommand(this);
            bool cmdStillProc = true;
            do {
                cmdStillProc = cmdEnum.MoveNext();

                // Process all decisions by command.
                {
                    IEnumerator decisionProcEnumerator = this.ProcessQueuedDecisions();
                    bool hasNext = true;
                    do {
                        hasNext = decisionProcEnumerator.MoveNext();
                        yield return 0;
                    } while (hasNext);
                }
            } while (cmdStillProc);

            // Let frontend process all game requests.
            if (this.CheckIfShouldYield()) {
                yield return 0;
            }
        }

        yield break;
    }
}
