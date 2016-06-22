using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class AllSameIdCombination : HandCombination {
    public AllSameIdCombination() : base("All Same Id", 1) { }

    public override bool HandHasCombination(List<Tile> tiles, HandCombination.CompletionType compType) {
        List<Tile> sets = Tile.ReturnGroupedTiles(tiles);

        HashSet<int> ids = new HashSet<int>();
        foreach(Tile tile in sets) {
            ids.Add(tile.Id);
        }

        bool valid = sets.Count == 3 && ids.Count == 1;

        return valid;
    }

    public override float GetProbabilityOfCompletion(List<Tile> _tiles, List<Tile.TileProp> allTileData, Game game, out List<Tile.TileProp> outTilePropsUsed) {
        List<Tile> tiles = new List<Tile>(_tiles);

        float highestProb = 0;

        List<Tile.TileProp> finalTilePropsUsed = new List<Tile.TileProp>();
        for (int i = 0; i < 2; ++i) {
            List<Tile.TileProp> tilePropsUsed;
            float prob = Tile.FindCompleteHandWithHighestProb(tiles.FindAll(t => t.Id == i), allTileData.FindAll(t => t.id == i), game, out tilePropsUsed);

            if (prob >= highestProb) {
                finalTilePropsUsed = tilePropsUsed;
                highestProb = prob;
            }
        }

        outTilePropsUsed = finalTilePropsUsed;
        return highestProb;
    }
}
