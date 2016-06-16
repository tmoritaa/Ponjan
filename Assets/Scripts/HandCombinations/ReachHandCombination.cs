using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ReachHandCombination : HandCombination {
    public ReachHandCombination() : base("Reach", 1) {}

    public override bool HandHasCombination(List<Tile> tiles) {
        Player player = tiles[0].Owner;

        return player.HasReached;
    }
}
