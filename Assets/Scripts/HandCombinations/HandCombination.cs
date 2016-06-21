using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public abstract class HandCombination {
    public enum CompletionType {
        Draw,
        Steal,
    }

    protected string name;
    public string Name {
        get { return this.name; }
    }

    protected int score;
    public int Score {
        get { return this.score; }
    }

    public HandCombination(string name, int score) {
        this.name = name;
        this.score = score;
    }

    public abstract bool HandHasCombination(List<Tile> tiles, CompletionType compType);

    public abstract int ReturnNumTilesToComplete(List<Tile> _tiles, out List<Tile> unnecessaryTiles);
}
