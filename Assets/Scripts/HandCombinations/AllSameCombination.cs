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

    public override float GetProbabilityOfCompletion(List<Tile> _tiles, List<Tile.TileProp> allTileData, Game game, out List<Tile.TileProp> outTilePropsUsed) {
        List<Tile> tiles = new List<Tile>(_tiles);

        float highestProb = 0;

        List<Tile.TileProp> finalTilePropsUsed = new List<Tile.TileProp>();
        List<Tile> nonDupTiles = Tile.ReturnTilesWithoutDuplicates(tiles);
        foreach (Tile tile in nonDupTiles) {
            List<Tile.TileProp> tilePropsUsed;
            float prob = Tile.FindCompleteHandWithHighestProb(tiles.FindAll(t => t.IsSame(tile)), allTileData.FindAll(t => t.Equals(tile)), game, out tilePropsUsed);

            if (prob > highestProb) {
                highestProb = prob;
                finalTilePropsUsed = tilePropsUsed;
            }
        }

        outTilePropsUsed = finalTilePropsUsed;
        return highestProb;
    }
}
