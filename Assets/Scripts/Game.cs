﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Game {
    private Dictionary<PhaseNode.PhaseID, PhaseNode> phaseNodes = new Dictionary<PhaseNode.PhaseID, PhaseNode>();
    private PhaseNode curPhase;
    private IEnumerator curPhaseEnumerator;

    private IEnumerator mainGameLoop;

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

    private bool CheckIfShouldYield() {
        return this.uiResponseRequests.Count > 0 || this.uiUpdateRequests.Count > 0;
    }

    public void ContinueMainGameLoop() {
        this.mainGameLoop.MoveNext();

        // TODO: this will have to be moved somewhere else, or at least not run for every game instance, since this will break when performing AI.
        CombatSceneController.Instance.RequestUpdates(this.uiUpdateRequests);
        CombatSceneController.Instance.RequestResponses(this.uiResponseRequests);
    }

    public void Initialize(Player.PlayerType[] playerTypes, string[] names, List<TileSetupData.TileSetupEntry> tileSetupEntries) {
        // Assuming there's only two players.
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
        PhaseNode changeActivePlayerPhase = new ChangeActivePlayerPhase();

        gameStartPhase.Next = drawPhase;
        drawPhase.Next = completeHandPhase;
        completeHandPhase.Next = discardPhase;
        discardPhase.Next = changeActivePlayerPhase;
        changeActivePlayerPhase.Next = drawPhase;

        this.phaseNodes[gameStartPhase.PhaseId] = gameStartPhase;
        this.phaseNodes[drawPhase.PhaseId] = drawPhase;
        this.phaseNodes[completeHandPhase.PhaseId] = completeHandPhase;
        this.phaseNodes[discardPhase.PhaseId] = discardPhase;
        this.phaseNodes[changeActivePlayerPhase.PhaseId] = changeActivePlayerPhase;
    }

    public void Start() {
        this.curPhase = this.phaseNodes[PhaseNode.PhaseID.StartGame];
        this.curPhaseEnumerator = this.curPhase.PerformPhase(this);

        this.mainGameLoop = this.Resume();
        this.ContinueMainGameLoop();
    }

    public IEnumerator Resume() {
        while (true) {
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
                this.curPhase = this.curPhase.Next;
                this.curPhaseEnumerator = this.curPhase.PerformPhase(this);
                // TODO: Request frontend to update.
                this.EnqueueUIUpdateRequest(new UIUpdateRequest(UIUpdateRequest.UpdateType.UpdateBoard));
                yield return 0;
            }
        }

        // yield break;
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