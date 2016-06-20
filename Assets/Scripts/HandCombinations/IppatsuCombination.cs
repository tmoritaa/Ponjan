using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class IppatsuCombination : HandCombination {
    public IppatsuCombination() : base("Ippatsu", 1) { }

    public override bool HandHasCombination(List<Tile> tiles, CompletionType compType) {
        Player player = tiles[0].Owner;

        List<Tile> sets = Tile.ReturnGroupedTiles(tiles);

        return sets.Count == 3 && player.IppatsuPotential;
    }

    public override int ReturnNumTilesToComplete(List<Tile> tiles) {
        return 100;
    }
}
