using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ReachHandCombination : HandCombination {
    public ReachHandCombination() : base("Reach", 1) {}

    public override bool HandHasCombination(List<Tile> tiles, HandCombination.CompletionType compType) {
        Player player = tiles[0].Owner;

        List<Tile> sets = Tile.ReturnGroupedTiles(tiles);

        return sets.Count == 3 && player.HasReached;
    }

    public override int ReturnNumTilesToComplete(List<Tile> _tiles, out List<Tile> outUnnecessaryTiles) {
        List<Tile> tiles = new List<Tile>(_tiles);

        List<Tile> sets = Tile.ReturnGroupedTiles(tiles, 3);
        foreach (Tile tile in sets) {
            for (int i = 0; i < 3; ++i) {
                tiles.Remove(tiles.Find(t => t.IsSame(tile)));
            }
        }

        List<Tile> pairs = Tile.ReturnGroupedTiles(tiles, 2);
        List<Tile> removedPairTiles = new List<Tile>();
        foreach (Tile tile in pairs) {
            for (int i = 0; i < 2; ++i) {
                Tile pairTile = tiles.Find(t => t.IsSame(tile));
                removedPairTiles.Add(pairTile);
                tiles.Remove(pairTile);
            }
        }

        List<Tile> singles = tiles;

        List<Tile> unnecTiles = new List<Tile>();
        if (singles.Count > 0) {
            unnecTiles = singles;
        } else if (pairs.Count > 0) {
            unnecTiles = removedPairTiles;
        }

        outUnnecessaryTiles = unnecTiles;
        return Tile.GetNumberOfTilesToCompleteHand(tiles);
    }
}
