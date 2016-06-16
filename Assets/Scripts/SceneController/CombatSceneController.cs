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

    [SerializeField]
    GameObject completeHandButtons;

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

        if (!this.gameInProgress) {
            this.gameInProgress = true;
            this.game.StartNewRound();
        } else if (this.curHandlingResponse.requestType == UIResponseRequest.ResponseType.None) {
            this.game.ContinueMainGameLoop();
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
                    break;
                case UIUpdateRequest.UpdateType.Reset:
                    this.Reset();
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
            }
        }
    }

    private void Reset() {
        this.curHandlingResponse.requestType = UIResponseRequest.ResponseType.None;
        this.gameInProgress = false;
        this.responsesToHandle.Clear();
    }

    private void Initialize() {
        this.game.Initialize(4, new Player.PlayerType[] { Player.PlayerType.Human, Player.PlayerType.Human, Player.PlayerType.Human }, new string[] { "Greg", "Fred", "Dred" }, tileSetupData.TileSetups);

        List<Player> players = this.game.Players;

        this.PlayerSize = this.game.Players.Count;

        foreach(TileSetupData.TileSetupEntry entry in this.tileSetupData.TileSetups) {
            this.MaxDeckSize += entry.numberInDeck;
        }

        // Number of players and player zones in list must be the same.
        for (int i = 0; i < this.playerZones.Count; ++i) {
            this.playerZones[i].Initialize(players[i]);
        }
    }

    public void TilePressed(Tile tile) {
        bool handled = this.game.HandleUIResponse(tile);

        if (handled) {
            this.curHandlingResponse.requestType = UIResponseRequest.ResponseType.None;
        }
    }

    public void ButtonPressed(string action) {
        bool handled = this.game.HandleUIResponse(action);

        if (handled) {
            this.curHandlingResponse.requestType = UIResponseRequest.ResponseType.None;
            this.completeHandButtons.SetActive(false);
        }
    }
}