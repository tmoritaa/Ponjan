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

    public override int ReturnNumTilesToComplete(List<Tile> tiles, out List<Tile> outUnnecessaryTiles) {
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

        Tile.TileType[] tileTypes = new Tile.TileType[3] { Tile.TileType.Red, Tile.TileType.Blue, Tile.TileType.Yellow };
        int[] neededTilePerNum = new int[3] { redNeededTileNum, blueNeededTileNum, yellowNeededTileNum };

        List<Tile.TileType> minTypes = new List<Tile.TileType>();
        int minCount = 999;
        for (int i = 0; i < 3; ++i) {
            int tileNum = neededTilePerNum[i];
            if (tileNum <= minCount) {
                if (tileNum < minCount) {
                    minTypes.Clear();
                    minCount = tileNum;
                }

                minTypes.Add(tileTypes[i]);
            }
        }

        List<Tile> unnecessaryTiles = new List<Tile>();
        foreach (Tile tile in tiles) {
            bool unnecessary = true;
            foreach (Tile.TileType type in minTypes) {
                if (tile.Type == type || tile.Type == Tile.TileType.White || tile.Type == Tile.TileType.Dragon) {
                    unnecessary = false;
                    break;
                }
            }

            if (unnecessary) {
                unnecessaryTiles.Add(tile);
            }
        }
        outUnnecessaryTiles = unnecessaryTiles;

        return minCount;
    }
}
