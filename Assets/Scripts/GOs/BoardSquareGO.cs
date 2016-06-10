using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BoardSquareGO : MonoBehaviour {
    [SerializeField]
    private SquareCardGO squareCardGO;

    [SerializeField]
    private List<Image> playerTokenGOs;

    private BoardSquareZone boardSquare = null;
    public BoardSquareZone BoardSquare {
        set { this.boardSquare = value; }
    }

    private Image boardSquareImage;
    public Image BoardSquareImage {
        get { return this.boardSquareImage; }
    }

    void Awake() {
        this.boardSquareImage = this.GetComponent<Image>();

        this.squareCardGO.gameObject.SetActive(false);
        this.playerTokenGOs.ForEach(o => o.gameObject.SetActive(false));
    }

    public void UpdateBoardSquare() {
        if (this.boardSquare == null) {
            return;
        }

        if (this.boardSquare.Cards.Count == 1) {
            this.squareCardGO.UpdateSquareCard(this.boardSquare.Cards[0]);
        }
        this.squareCardGO.gameObject.SetActive(this.boardSquare.Cards.Count == 1);

        for (int i = 0; i < this.playerTokenGOs.Count; ++i) {
            this.playerTokenGOs[i].gameObject.SetActive(this.boardSquare.Players[i] != null);
        }
    }

    public void BoardSquareClicked() {
        if (this.boardSquare != null) {
            CombatSceneController.Instance.BoardSquarePressed(this.boardSquare);
        }
    }
}
