using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class BoardSquareZone : Zone {
    public enum RangeType {
        None,
        Adjacent,
    }

    private int boardIdx = 0;

    private Player[] players = new Player[2] { null, null };
    public Player[] Players {
        get { return this.players; }
    }

    private struct XYCoord {
        public int x;
        public int y;
    }

    public BoardSquareZone(int idx) : base(ZoneType.Gameboard) {
        this.boardIdx = idx;
    }

    private void GetAllSquaresInRange(BoardSquareZone root, RangeType rangeType, GameBoard board, HashSet<BoardSquareZone> zonesInRange) {
        zonesInRange.Add(root);

        bool[] range = new bool[0];

        const bool T = true;
        const bool F = false;
        switch (rangeType) {
            case RangeType.None:
                range = new bool[] { F };
                break;
            case RangeType.Adjacent:
                range = new bool[] { F, T, F, T, T, T, F, T, F };
                break;
        }

        int midIdx = range.Length / 2;
        int rangeDim = Mathf.RoundToInt(Mathf.Sqrt((float)range.Length));
        List<XYCoord> coords = new List<XYCoord>();
        for (int i = 0; i < range.Length; ++i) {
            if (range[i]) {
                XYCoord coord;
                coord.x = (i - midIdx) % rangeDim;
                coord.y = (i - midIdx) / rangeDim;
                coords.Add(coord);
            }
        }

        int boardDim = Mathf.RoundToInt(Mathf.Sqrt((float)board.Squares.Length));
        foreach (XYCoord coord in coords) {
            int boardIdx = root.boardIdx + (coord.x + coord.y * boardDim);

            if (boardIdx >= 0 && boardIdx < board.Squares.Length && !zonesInRange.Contains(board.Squares[boardIdx])) {
                zonesInRange.Add(board.Squares[boardIdx]);
            }
        }
    }

    public HashSet<BoardSquareZone> GetSquaresInRange(RangeType rangeType, GameBoard board) {
        HashSet<BoardSquareZone> zonesInRange = new HashSet<BoardSquareZone>();
        this.GetAllSquaresInRange(this, rangeType, board, zonesInRange);
        return zonesInRange;
    }

    public bool ZoneIsInRange(BoardSquareZone zone, RangeType rangeType, GameBoard board) {
        HashSet<BoardSquareZone> zonesInRange = this.GetSquaresInRange(rangeType, board);
        return zonesInRange.Contains(zone);
    }

    public override void AddCard(Card card) {
        Debug.Assert(this.cards.Count == 0, "Card was added to BoardSquareZone when card was already there.");
        
        base.AddCard(card);
    }

    public bool CanBePlayedOn(Player player) {
        return this.Cards.Count == 0 || this.Cards[0].Owner == player;
    }
}