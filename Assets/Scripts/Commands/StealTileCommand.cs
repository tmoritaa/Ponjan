using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class StealTileCommand : Command {
    private Player player;
    private Tile tile;

    public StealTileCommand(Player player, Tile tile) : base() {
        this.player = player;
        this.tile = tile;
    }

    public override IEnumerator PerformCommand(Game game) {
        this.player.StealTileFromDiscard(this.tile);
        yield break;
    }
}
