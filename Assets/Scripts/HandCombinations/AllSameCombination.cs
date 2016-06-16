using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class AllSameCombination : HandCombination {
    public AllSameCombination() : base("All Same", 13) { }

    public override bool HandHasCombination(List<Tile> tiles) {
        List<Tile> sets = Tile.ReturnGroupedTiles(tiles);

        if (sets.Count != 3) {
            return false;
        }

        bool allSame = true;
        Tile tile = sets[0];
        for (int i = 1; i < sets.Count; ++i) {
            Tile otherTile = sets[i];
            
            if (!tile.IsSame(otherTile)) {
                allSame = false;
                break;
            }
        }
        
        bool valid = sets.Count == 3 && allSame;

        return valid;
    }
}
