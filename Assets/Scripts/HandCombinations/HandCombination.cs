using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public abstract class HandCombination {
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

    public abstract bool HandHasCombination(List<Tile> tiles);

    protected List<Tile> FindSets(List<Tile> tiles) {
        List<Tile> sets = new List<Tile>();

        tiles.Sort(Tile.CompareTiles);

        int counter = 1;
        Tile curTile = tiles[0];
        for (int i = 1; i < tiles.Count; ++i) {
            if (curTile == null) {
                curTile = tiles[i];
                counter = 1;
                continue;
            }

            Tile tile = tiles[i];
            if (curTile.IsSame(tile)) {
                ++counter;
            } else {
                counter = 1;
                curTile = tile;
            }

            if (counter >= 3) {
                sets.Add(curTile);
                curTile = null;
            }
        }

        return sets;
    }
}
