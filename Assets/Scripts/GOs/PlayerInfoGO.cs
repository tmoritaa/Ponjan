using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerInfoGO : MonoBehaviour {
    [SerializeField]
    private Text playerInfoText;

    private Player player = null;
    public Player Player {
        set { this.player = value; }
    }

	void Update() {
        if (this.player != null) {
            this.playerInfoText.text = "Name: " + player.Name + "\nHealth: " + player.CurHealth + "\nMaterial: " + player.CurMaterial + "\nHasInitiative: " + player.HasInitiative + "\nDiscard: " + player.Discard.Cards.Count;
        }
    }
}
