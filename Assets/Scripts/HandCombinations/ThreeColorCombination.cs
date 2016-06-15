using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ThreeColorCombination : HandCombination {
    public ThreeColorCombination() : base("Three Colors", 2) { }

    public override bool HandHasCombination(List<Tile> tiles) {
        List<Tile> sets = this.FindSets(tiles);

        return (sets.Count == 3 && 
            sets.Exists(t => t.Type == Tile.TileType.Red) && 
            sets.Exists(t => t.Type == Tile.TileType.Blue) && 
            sets.Exists(t => t.Type == Tile.TileType.Yellow));
    }
}
