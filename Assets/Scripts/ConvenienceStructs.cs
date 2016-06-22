using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public struct TileDrawProb {
    public Tile.TileProp tileProp;
    public float prob;

    public TileDrawProb(Tile.TileProp prop, float prob) {
        this.tileProp = prop;
        this.prob = prob;
    }

    public static int SortFunc(TileDrawProb x, TileDrawProb y) {
        if (x.prob < y.prob) {
            return 1;
        } else if (x.prob > y.prob) {
            return -1;
        } else {
            return 0;
        }
    }
}

public struct HandCombProbs {
    public HandCombination handComb;
    public List<Tile.TileProp> necessaryTiles;
    public float prob;

    public HandCombProbs(HandCombination handComb, List<Tile.TileProp> necessaryTiles, float prob) {
        this.handComb = handComb;
        this.necessaryTiles = necessaryTiles;
        this.prob = prob;
    }

    public static int SortFunc(HandCombProbs x, HandCombProbs y) {
        if (x.prob < y.prob) {
            return 1;
        } else if (x.prob > y.prob) {
            return -1;
        } else {
            return 0;
        }
    }
}
