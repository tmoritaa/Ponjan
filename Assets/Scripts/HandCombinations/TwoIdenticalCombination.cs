using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class TwoIdenticalCombination : HandCombination {
    public TwoIdenticalCombination() : base("Two Identical", 1) { }

    public override bool HandHasCombination(List<Tile> tiles, HandCombination.CompletionType compType) {
        List<Tile> sets = Tile.ReturnGroupedTiles(tiles);

        bool identicalFound = false;
        for (int i = 0; i < sets.Count; ++i) {
            Tile tile = sets[i];
            for (int j = 0; j < sets.Count; ++j) {
                if (i == j) {
                    continue;
                }

                Tile otherTile = sets[j];

                if (tile.IsSame(otherTile)) {
                    identicalFound = true;
                    break;
                }
            }

            if (identicalFound) {
                break;
            }
        }
        

        bool valid = sets.Count == 3 && identicalFound;

        return valid;
    }

    public override int ReturnNumTilesToComplete(List<Tile> tiles) {
        int highestRed = Tile.GetHighestNumOfSameTilesOfType(tiles, Tile.TileType.Red);
        int highestBlue = Tile.GetHighestNumOfSameTilesOfType(tiles, Tile.TileType.Blue);
        int highestYellow = Tile.GetHighestNumOfSameTilesOfType(tiles, Tile.TileType.Yellow);

        int max = Math.Max(Math.Max(highestRed, highestBlue), highestYellow);

        int identicalSize = CombatSceneController.SetSize * 2;

        return identicalSize - Math.Min(max, identicalSize);
    }
}
