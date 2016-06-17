using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class AllSameColorCombination : HandCombination {
    public AllSameColorCombination() : base("All Same Color", 3) { }

    public override bool HandHasCombination(List<Tile> tiles, HandCombination.CompletionType compType) {
        List<Tile> sets = Tile.ReturnGroupedTiles(tiles);

        int redExists = sets.Exists(t => t.Type == Tile.TileType.Red) ? 1 : 0;
        int blueExists = sets.Exists(t => t.Type == Tile.TileType.Blue) ? 1 : 0;
        int yellowExists = sets.Exists(t => t.Type == Tile.TileType.Yellow) ? 1 : 0;
        bool whiteExists = sets.Exists(t => t.Type == Tile.TileType.White);
        bool dragonExists = sets.Exists(t => t.Type == Tile.TileType.Dragon);

        int numColors = redExists + blueExists + yellowExists;

        bool valid = sets.Count == 3 && numColors == 1 && !whiteExists && !dragonExists;

        return valid;
    }
}
