using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class AllSameCombination : HandCombination {
    public AllSameCombination() : base("All Same", 12) { }

    public override bool HandHasCombination(List<Tile> tiles, HandCombination.CompletionType compType) {
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

    public override int ReturnNumTilesToComplete(List<Tile> tiles) {
        List<Tile> tileTypes = Tile.ReturnGroupedTiles(tiles, 1);

        int num = 0;
        foreach(Tile tile in tileTypes) {
            if (tile.Type == Tile.TileType.Dragon || tile.Type == Tile.TileType.White) {
                continue;
            }

            int count = tiles.FindAll(t => t.IsSame(tile)).Count;
            if (count > num) {
                num = count;
            }
        }

        return CombatSceneController.MaxPlayerHandSize - num;
    }
}
