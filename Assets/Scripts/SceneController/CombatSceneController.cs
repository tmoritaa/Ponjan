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
    private TileSetupData tileSetupData;

	// Use this for initialization
    // TODO: should be Awake. Only because we need CardInfoManager to be initialized first, which will always be true once it gets moved to other scene.
	void Start () {
        CombatSceneController.instance = this;

        this.game.Initialize(new Player.PlayerType[] { Player.PlayerType.Human, Player.PlayerType.Human }, new string[] { "Greg", "Fred" }, tileSetupData.TileSetups);

        this.game.Start();
	}

    void OnDestroy() {
        CombatSceneController.instance = null;
    }

    void Update() {
        this.HandleUIUpdates();
        this.HandleResponse();

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

    private void HandleUIUpdates() {
        while(this.updatesToHandle.Count > 0) {
            UIUpdateRequest request = this.updatesToHandle.Dequeue();
            switch (request.requestType) {
                case UIUpdateRequest.UpdateType.None:
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
                    break;
            }
        }
    }
}