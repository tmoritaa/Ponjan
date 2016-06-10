using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandGO : MonoBehaviour {
    [SerializeField]
    List<CardGO> cardGOs;

    private Player player = null;
    public Player Player {
        set { this.player = value; }
    }

    void Start() {
        foreach(CardGO cgo in this.cardGOs) {
            cgo.gameObject.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
	    if (this.player == null) {
            return;
        }

        List<Card> hand = this.player.Hand.Cards;
        for(int i = 0; i < this.cardGOs.Count; ++i) {
            if (i < hand.Count) {
                this.cardGOs[i].Card = hand[i];
                this.cardGOs[i].gameObject.SetActive(true);
            } else {
                this.cardGOs[i].gameObject.SetActive(false);
            }
        }
	}
}
