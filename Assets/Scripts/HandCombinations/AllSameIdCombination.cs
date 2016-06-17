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
}
