using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameBoardGO : MonoBehaviour {
    [SerializeField]
    private BoardSquareGO boardSquareGOPrefab;

    private BoardSquareGO[] boardSquareGOs;

    private GameBoard gameboard;
    public GameBoard GameBoard {
        set {
            this.gameboard = value;
            this.GenerateBoard();
        }
    }    

    private void GenerateBoard() {
        this.boardSquareGOs = new BoardSquareGO[this.gameboard.Squares.Length];

        // Assuming game board dimension is odd.
        int dim = Mathf.RoundToInt(Mathf.Sqrt((float)this.gameboard.Squares.Length));
        for(int i = 0; i < this.boardSquareGOs.Length; ++i) {
            BoardSquareGO boardSquareGO = GameObject.Instantiate<BoardSquareGO>(this.boardSquareGOPrefab);
            boardSquareGO.transform.SetParent(this.transform, false);

            int x = (i % dim) - (int)(dim / 2);
            int y = i / dim - (int)(dim / 2);

            Vector3 pos = new Vector2(x * boardSquareGO.BoardSquareImage.rectTransform.rect.width, y * boardSquareGO.BoardSquareImage.rectTransform.rect.height);
            boardSquareGO.transform.localPosition = pos;
            boardSquareGO.BoardSquare = this.gameboard.Squares[i];
            this.boardSquareGOs[i] = boardSquareGO;
        }
    }

    public void UpdateBoard() {
        for (int i = 0; i < this.boardSquareGOs.Length; ++i) {
            this.boardSquareGOs[i].UpdateBoardSquare();
        }
    }
}
