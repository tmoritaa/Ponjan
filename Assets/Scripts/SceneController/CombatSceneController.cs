using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CombatSceneController : MonoBehaviour {
    [SerializeField]
    private int boardDimension = 5;
    public int BoardDimension {
        get { return this.boardDimension; }
    }

    [SerializeField]
    private GameBoardGO gameboardGO;

    [SerializeField]
    private Text phaseText = null;

    [SerializeField]
    private List<PlayerZoneGO> playerInfoGOs;

    [SerializeField]
    private GameObject buttons = null;

    private Queue<UIResponseRequest> responsesToHandle = new Queue<UIResponseRequest>();
    private UIResponseRequest curHandlingResponse = new UIResponseRequest(UIResponseRequest.ResponseType.None);

    private Queue<UIUpdateRequest> updatesToHandle = new Queue<UIUpdateRequest>();

    private Game game = new Game();

    private static CombatSceneController instance;
    public static CombatSceneController Instance {
        get { return CombatSceneController.instance; }
    }

	// Use this for initialization
    // TODO: should be Awake. Only because we need CardInfoManager to be initialized first, which will always be true once it gets moved to other scene.
	void Start () {
        CombatSceneController.instance = this;

        this.buttons.SetActive(false);

        this.game.Initialize(new Player.PlayerType[] { Player.PlayerType.Human, Player.PlayerType.Human }, new string[] { "Greg", "Fred" });

        // Assuming two players and two playerInfoGOs.
        for(int i = 0; i < 2; ++i) {
            this.playerInfoGOs[i].Initialize(this.game.Players[i]);
        }

        this.gameboardGO.GameBoard = this.game.Board;

        this.game.Start();
	}

    void OnDestroy() {
        CombatSceneController.instance = null;
    }

    void Update() {
        this.HandleUIUpdates();
        this.HandleResponse();

        this.phaseText.text = this.game.GetCurrentPhaseText();

        if (this.curHandlingResponse.requestType == UIResponseRequest.ResponseType.None) {
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

    private void HandleUpdateBoardRequest() {
        this.gameboardGO.UpdateBoard();
    }

    private void HandleUIUpdates() {
        while(this.updatesToHandle.Count > 0) {
            UIUpdateRequest request = this.updatesToHandle.Dequeue();
            switch (request.requestType) {
                case UIUpdateRequest.UpdateType.UpdateBoard:
                    this.HandleUpdateBoardRequest();
                    break;
            }
        }
    }

    private void PreparePlayPhaseActionResponse() {
        this.buttons.SetActive(true);

        // Possible responses are:
        // - Card in hand = Tactics or unit card
        // - Card in BoardSquare = activated ability
        // - Player token = movement
        // - pass button = pass
    }

    private void HandleResponse() {
        while (this.curHandlingResponse.requestType == UIResponseRequest.ResponseType.None && this.responsesToHandle.Count > 0) {
            UIResponseRequest entry = this.responsesToHandle.Dequeue();
            this.curHandlingResponse = entry;

            switch (entry.requestType) {
                case UIResponseRequest.ResponseType.PlayPhaseAction:
                    this.PreparePlayPhaseActionResponse();
                    break;
            }
        }
    }

    private void PlayPhaseHandled() {
        this.buttons.SetActive(false);
        this.curHandlingResponse.requestType = UIResponseRequest.ResponseType.None;
    }

    public void ActionPressed(string str) {
        bool valid = this.game.HandleUIResponse(str);
        if (valid) {
            this.PlayPhaseHandled();
        }
    }

    public void CardPressed(Card card) {
        bool valid = this.game.HandleUIResponse(card);
        if (valid) {
            this.PlayPhaseHandled();
        }
    }

    public void BoardSquarePressed(BoardSquareZone boardSquare) {
        bool valid = this.game.HandleUIResponse(boardSquare);
        if (valid) {
            this.PlayPhaseHandled();
        }
    }
}