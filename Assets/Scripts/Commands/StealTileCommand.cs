using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class StealTileCommand : Command {
    private Player stolenPlayer;
    private Player stealingPlayer;
    private Tile tile;

    public StealTileCommand(Player stolenPlayer, Player stealingPlayer, Tile tile) : base() {
        this.stolenPlayer = stolenPlayer;
        this.stealingPlayer = stealingPlayer;
        this.tile = tile;
    }

    public override IEnumerator PerformCommand(Game game) {
        this.stealingPlayer.StealTileFromDiscard(this.tile, this.stolenPlayer);
        yield break;
    }
}
