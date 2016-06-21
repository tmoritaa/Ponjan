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

    public override int ReturnNumTilesToComplete(List<Tile> tiles, out List<Tile> outUnnecessaryTiles) {
        List<Tile> redDupTiles;
        List<Tile> blueDupTiles;
        List<Tile> yellowDupTiles;
        int redNum = CombatSceneController.SetSize - Math.Min(Tile.GetHighestNumOfSameTilesOfType(tiles, Tile.TileType.Red, out redDupTiles), CombatSceneController.SetSize);
        int blueNum = CombatSceneController.SetSize - Math.Min(Tile.GetHighestNumOfSameTilesOfType(tiles, Tile.TileType.Blue, out blueDupTiles), CombatSceneController.SetSize);
        int yellowNum = CombatSceneController.SetSize - Math.Min(Tile.GetHighestNumOfSameTilesOfType(tiles, Tile.TileType.Yellow, out yellowDupTiles), CombatSceneController.SetSize);

        List<Tile> unnecessaryTiles = tiles.FindAll(t => t.Type == Tile.TileType.Dragon || t.Type == Tile.TileType.White);

        Tile.TileType[] tileTypes = new Tile.TileType[3] { Tile.TileType.Red, Tile.TileType.Blue, Tile.TileType.Yellow };
        foreach (Tile.TileType type in tileTypes) {
            List<Tile> colorTiles = tiles.FindAll(t => t.Type == type);
            List<Tile> sets = Tile.ReturnGroupedTiles(colorTiles, 3);

            if (sets.Count > 0) {
                for (int i = 0; i < CombatSceneController.SetSize; ++i) {
                    colorTiles.Remove(colorTiles.Find(t => t.IsSame(sets[0])));
                }

                unnecessaryTiles.AddRange(colorTiles);
            }
        }

        outUnnecessaryTiles = unnecessaryTiles;
        return redNum + blueNum + yellowNum;
    }
}
