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

    public override int ReturnNumTilesToComplete(List<Tile> tiles, out List<Tile> outUnnecessaryTiles) {
        List<Tile> redDupSets;
        List<Tile> blueDupSets;
        List<Tile> yellowDupSets;
        int highestRed = Tile.GetHighestNumOfSameTilesOfType(tiles, Tile.TileType.Red, out redDupSets);
        int highestBlue = Tile.GetHighestNumOfSameTilesOfType(tiles, Tile.TileType.Blue, out blueDupSets);
        int highestYellow = Tile.GetHighestNumOfSameTilesOfType(tiles, Tile.TileType.Yellow, out yellowDupSets);

        int highestNum = 0;
        List<List<Tile>> outDupSetList = new List<List<Tile>>();
        List<Tile>[] dupSetArr = new List<Tile>[3] { redDupSets, blueDupSets, yellowDupSets };
        int[] numPerType = new int[3] { highestRed, highestBlue, highestYellow };
        for (int i = 0; i < 3; ++i) {
            if (highestNum <= numPerType[i]) {
                if (highestNum < numPerType[i]) {
                    outDupSetList.Clear();
                    highestNum = numPerType[i];
                }
                outDupSetList.Add(dupSetArr[i]);
            }
        }

        List<Tile> unnecTiles = new List<Tile>();

        foreach(Tile tile in tiles) {
            bool isUnnec = true;
            foreach (List<Tile> dupSets in outDupSetList) {
                foreach (Tile setTile in dupSets) {
                    if (setTile.IsSame(tile)) {
                        isUnnec = false;
                        break;
                    }
                }

                if (!isUnnec) {
                    break;
                }
            }

            if (isUnnec) {
                unnecTiles.Add(tile);
            }
        }
        


        int identicalSize = CombatSceneController.SetSize * 2;

        outUnnecessaryTiles = unnecTiles;

        return identicalSize - Math.Min(highestNum, identicalSize);
    }
}
