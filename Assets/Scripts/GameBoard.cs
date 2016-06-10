using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class GameBoard {
    private BoardSquareZone[] squares;
    public BoardSquareZone[] Squares {
        get { return this.squares; }
    }

    public GameBoard(int boardDim) {
        this.squares = new BoardSquareZone[boardDim * boardDim];
        for(int i = 0; i < boardDim * boardDim; ++i) {
            this.squares[i] = new BoardSquareZone(i);
        }
    }
}
