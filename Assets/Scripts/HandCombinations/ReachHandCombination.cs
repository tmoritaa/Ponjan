using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ReachHandCombination : HandCombination {
    public ReachHandCombination() : base("Reach", 1) {}

    public override bool HandHasCombination(List<Tile> tiles, HandCombination.CompletionType compType) {
        Player player = tiles[0].Owner;

        List<Tile> sets = Tile.ReturnGroupedTiles(tiles);

        return sets.Count == 3 && player.HasReached;
    }

    public override float GetProbabilityOfCompletion(List<Tile> _tiles, List<Tile.TileProp> allTileData, Game game, out List<Tile.TileProp> outTilePropsUsed) {
        List<Tile.TileProp> tilePropsUsed;
        float prob = Tile.FindCompleteHandWithHighestProb(new List<Tile>(_tiles), allTileData, game, out tilePropsUsed);

        outTilePropsUsed = tilePropsUsed;
        return prob;
    }
}
