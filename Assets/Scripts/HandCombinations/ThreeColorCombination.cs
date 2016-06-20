using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ThreeColorCombination : HandCombination {
    public ThreeColorCombination() : base("Three Colors", 2) { }

    public override bool HandHasCombination(List<Tile> tiles, HandCombination.CompletionType compType) {
        List<Tile> sets = Tile.ReturnGroupedTiles(tiles);

        return (sets.Count == 3 && 
            sets.Exists(t => t.Type == Tile.TileType.Red) && 
            sets.Exists(t => t.Type == Tile.TileType.Blue) && 
            sets.Exists(t => t.Type == Tile.TileType.Yellow));
    }

    public override int ReturnNumTilesToComplete(List<Tile> tiles) {
        int redNum = CombatSceneController.SetSize - Math.Min(Tile.GetHighestNumOfSameTilesOfType(tiles, Tile.TileType.Red), CombatSceneController.SetSize);
        int blueNum = CombatSceneController.SetSize - Math.Min(Tile.GetHighestNumOfSameTilesOfType(tiles, Tile.TileType.Blue), CombatSceneController.SetSize);
        int yellowNum = CombatSceneController.SetSize - Math.Min(Tile.GetHighestNumOfSameTilesOfType(tiles, Tile.TileType.Yellow), CombatSceneController.SetSize);

        return redNum + blueNum + yellowNum;
    }
}
