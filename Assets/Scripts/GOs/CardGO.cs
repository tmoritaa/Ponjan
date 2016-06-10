using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CardGO : MonoBehaviour {
    [SerializeField]
    Text text;

    private Card card = null;
    public Card Card {
        set { this.card = value; }
    }

    void Update() {
        if (this.card != null) {
            this.text.text = this.card.Info.Name;
        }
    }
    
    public void CardClicked() {
        if (this.card != null) {
            CombatSceneController.Instance.CardPressed(card);
        }
    }
}
