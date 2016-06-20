using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class OneColorAndWhitesCombination : HandCombination {
    public OneColorAndWhitesCombination() : base("One Color and Whites", 2) { }

    public override bool HandHasCombination(List<Tile> tiles, HandCombination.CompletionType compType) {
        List<Tile> sets = Tile.ReturnGroupedTiles(tiles);

        int redExists = sets.Exists(t => t.Type == Tile.TileType.Red) ? 1 : 0;
        int blueExists = sets.Exists(t => t.Type == Tile.TileType.Blue) ? 1 : 0;
        int yellowExists = sets.Exists(t => t.Type == Tile.TileType.Yellow) ? 1 : 0;
        bool whiteExists = sets.Exists(t => t.Type == Tile.TileType.White);
        bool dragonExists = sets.Exists(t => t.Type == Tile.TileType.Dragon);

        int numColors = redExists + blueExists + yellowExists;

        bool valid = sets.Count == 3 && numColors == 1 && (whiteExists || dragonExists);

        return valid;
    }

    public override int ReturnNumTilesToComplete(List<Tile> tiles) {
        List<Tile> redTiles = tiles.FindAll(t => t.Type == Tile.TileType.Red);
        List<Tile> blueTiles = tiles.FindAll(t => t.Type == Tile.TileType.Blue);
        List<Tile> yellowTiles = tiles.FindAll(t => t.Type == Tile.TileType.Yellow);
        List<Tile> otherTiles = tiles.FindAll(t => t.Type == Tile.TileType.White || t.Type == Tile.TileType.Dragon);

        redTiles.AddRange(otherTiles);
        blueTiles.AddRange(otherTiles);
        yellowTiles.AddRange(otherTiles);

        int redNeededTileNum = Tile.GetNumberOfTilesToCompleteHand(redTiles);
        int blueNeededTileNum = Tile.GetNumberOfTilesToCompleteHand(blueTiles);
        int yellowNeededTileNum = Tile.GetNumberOfTilesToCompleteHand(yellowTiles);

        int minCount = Math.Min(Math.Min(redNeededTileNum, blueNeededTileNum), yellowNeededTileNum);

        return minCount;
    }
}
