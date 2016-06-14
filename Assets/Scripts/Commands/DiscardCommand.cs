using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DiscardCommand : Command {
    private Player player;
    private Tile tile;

    public DiscardCommand(Player player, Tile tile) : base() {
        this.player = player;
        this.tile = tile;
    }

    public override IEnumerator PerformCommand(Game game) {
        this.player.DiscardFromHand(this.tile);
        this.player.SortHand();

        yield break;
    }

}
