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

    public override int ReturnNumTilesToComplete(List<Tile> tiles, out List<Tile> outUnnecessaryTiles) {
        int num = 99;
        List<int> goodIds = new List<int>();
        for (int i = 0; i < 2; ++i) {
            List<Tile> tilesWithId = tiles.FindAll(t => t.Id == i);

            int count = Tile.GetNumberOfTilesToCompleteHand(tilesWithId);
            if (count <= num) {
                if (count < num) {
                    goodIds.Clear();
                    num = count;
                }

                goodIds.Add(i);
            }
        }

        List<Tile> unnecTiles = new List<Tile>();
        foreach (Tile tile in tiles) {
            bool unnec = true;
            foreach (int id in goodIds) {
                if (tile.Id == id) {
                    unnec = false;
                    break;
                }
            }

            if (unnec) {
                unnecTiles.Add(tile);
            }
        }
        outUnnecessaryTiles = unnecTiles;

        return num;
    }
}
