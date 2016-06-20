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

    public override int ReturnNumTilesToComplete(List<Tile> tiles) {
        // Just a big number.
        return 100;
    }
}
