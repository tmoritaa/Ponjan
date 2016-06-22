using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CompleteWithDrawCombination : HandCombination {
    public CompleteWithDrawCombination() : base("Complete with Draw", 1) {}

    public override bool HandHasCombination(List<Tile> tiles, CompletionType compType) {
        List<Tile> sets = Tile.ReturnGroupedTiles(tiles);

        return sets.Count == 3 && compType == CompletionType.Draw;
    }

    public override float GetProbabilityOfCompletion(List<Tile> _tiles, List<Tile.TileProp> allTileData, Game game, out List<Tile.TileProp> outTilePropsUsed) {
        outTilePropsUsed = new List<Tile.TileProp>();
        return 0;
    }
}
