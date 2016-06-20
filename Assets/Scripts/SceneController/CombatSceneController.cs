using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CombatSceneController : MonoBehaviour {
    private Queue<UIResponseRequest> responsesToHandle = new Queue<UIResponseRequest>();
    private UIResponseRequest curHandlingResponse = new UIResponseRequest(UIResponseRequest.ResponseType.None);

    private Queue<UIUpdateRequest> updatesToHandle = new Queue<UIUpdateRequest>();

    private Game game = new Game();

    private static CombatSceneController instance;
    public static CombatSceneController Instance {
        get { return CombatSceneController.instance; }
    }

    public const int SetSize = 3;

    [SerializeField]
    public const int MaxPlayerHandSize = 9;

    [HideInInspector]
    public int MaxDeckSize = 0;

    [HideInInspector]
    public int PlayerSize = 0;

    [SerializeField]
    private TileSetupData tileSetupData;

    [SerializeField]
    private TileGO tilePrefab;
    public TileGO TilePrefab {
        get { return this.tilePrefab; }
    }

    // Must be in order of S, E, N, W.
    [SerializeField]
    List<PlayerZoneGO> playerZones;

    // Probably should come up with a better system for these buttons.
    [SerializeField]
    GameObject completeHandButtons;
    [SerializeField]
    GameObject stealTileButtons;
    [SerializeField]
    GameObject reachButton;
    [SerializeField]
    RoundResultsScreen roundResultsScreen;
    [SerializeField]
    ScoreDisplayGO scoreDisplayGO;
    [SerializeField]
    GameResultsScreen gameResultsScreen;

    bool gameInProgress = false;

	void Awake() {
        CombatSceneController.instance = this;

        this.Initialize();
	}

    void OnDestroy() {
        CombatSceneController.instance = null;
    }

    void Update() {
        this.HandleUIUpdates();
        this.HandleResponse();

        if (this.curHandlingResponse.requestType == UIResponseRequest.ResponseType.None) {
            if (!this.gameInProgress) {
                this.gameInProgress = true;
                this.game.StartNewRound();
            } else {
                this.game.ContinueMainGameLoop();
            }
        }
    }

    public void RequestResponses(Queue<UIResponseRequest> responses) {
        while (responses.Count > 0) {
            this.responsesToHandle.Enqueue(responses.Dequeue());
        }
    }

    public void RequestUpdates(Queue<UIUpdateRequest> updates) {
        while (updates.Count > 0) {
            this.updatesToHandle.Enqueue(updates.Dequeue());
        }
    }

    private void HandleUIUpdates() {
        while(this.updatesToHandle.Count > 0) {
            UIUpdateRequest request = this.updatesToHandle.Dequeue();
            switch (request.requestType) {
                case UIUpdateRequest.UpdateType.None:
                    break;
                case UIUpdateRequest.UpdateType.UpdateBoard:
                    this.playerZones.ForEach(z => z.UpdateZones(this.game));
                    this.scoreDisplayGO.UpdateText(this.game.Players, this.game.CurRound);
                    break;
                case UIUpdateRequest.UpdateType.Reset:
                    this.Reset();
                    break;
                case UIUpdateRequest.UpdateType.DisplayFinalResults:
                    this.gameResultsScreen.DisplayFinalResults(this.game.Players);
                    this.gameResultsScreen.gameObject.SetActive(true);
                    break;
            }
        }
    }

    private void HandleResponse() {
        while (this.curHandlingResponse.requestType == UIResponseRequest.ResponseType.None && this.responsesToHandle.Count > 0) {
            UIResponseRequest entry = this.responsesToHandle.Dequeue();
            this.curHandlingResponse = entry;

            switch (entry.requestType) {
                case UIResponseRequest.ResponseType.None:
                case UIResponseRequest.ResponseType.SelectTile:
                    break;
                case UIResponseRequest.ResponseType.DecideToComplete:
                    this.completeHandButtons.SetActive(true);
                    break;
                case UIResponseRequest.ResponseType.DecideToSteal:
                    this.stealTileButtons.SetActive(true);
                    break;
                case UIResponseRequest.ResponseType.SelectTileOrReach:
                    this.reachButton.SetActive(true);
                    break;
                case UIResponseRequest.ResponseType.DisplayCompletedHand:
                    this.DisplayCompleteHand(entry.objs);
                    break;
                case UIResponseRequest.ResponseType.DisplayNoDeckScoring:
                    this.DisplayNoDeckScoring();
                    break;
            }
        }
    }

    private void DisplayCompleteHand(List<object> objs) {
        // Parse input.
        Player player = (Player)objs[0];
        List<HandCombination> handCombs = new List<HandCombination>();
        for (int i = 1; i < objs.Count; ++i) {
            handCombs.Add((HandCombination)objs[i]);
        }

        // Pass to game object responsible for displaying.
        this.roundResultsScreen.UpdateResultsForHandCombDisplay(player, handCombs);

        // Set game object to be displayed.        
        this.roundResultsScreen.gameObject.SetActive(true);
    }

    private void DisplayNoDeckScoring() {
        this.roundResultsScreen.UpdateResultsForNoDeckScoring(this.game.Players);

        // Set game object to be displayed.        
        this.roundResultsScreen.gameObject.SetActive(true);
    }

    private void Reset() {
        this.curHandlingResponse.requestType = UIResponseRequest.ResponseType.None;
        this.gameInProgress = false;
        this.responsesToHandle.Clear();
    }

    private void Initialize() {
        this.game.Initialize(4, new Player.PlayerType[] { Player.PlayerType.Human, Player.PlayerType.AI, Player.PlayerType.AI }, new string[] { "Greg", "Fred", "Dred" }, tileSetupData.TileSetups);

        List<Player> players = this.game.Players;

        this.PlayerSize = this.game.Players.Count;

        foreach(TileSetupData.TileSetupEntry entry in this.tileSetupData.TileSetups) {
            this.MaxDeckSize += entry.numberInDeck;
        }

        // Number of players and player zones in list must be the same.
        for (int i = 0; i < this.playerZones.Count; ++i) {
            this.playerZones[i].Initialize(players[i]);
        }

        this.gameResultsScreen.gameObject.SetActive(false);
        this.roundResultsScreen.gameObject.SetActive(false);
        this.completeHandButtons.gameObject.SetActive(false);
        this.stealTileButtons.gameObject.SetActive(false);
        this.reachButton.gameObject.SetActive(false);
    }

    public void TilePressed(Tile tile) {
        bool handled = this.game.HandleUIResponse(tile);

        if (handled) {
            this.ResponseHandled();
        }
    }

    public void ButtonPressed(string action) {
        bool handled = this.game.HandleUIResponse(action);

        if (handled) {
            this.ResponseHandled();
        }
    }

    private void ResponseHandled() {
        this.curHandlingResponse.requestType = UIResponseRequest.ResponseType.None;
        this.completeHandButtons.SetActive(false);
        this.stealTileButtons.SetActive(false);
        this.reachButton.SetActive(false);
        this.roundResultsScreen.gameObject.SetActive(false);
    }
}