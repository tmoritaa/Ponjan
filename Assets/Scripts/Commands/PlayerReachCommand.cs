using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PlayerReachCommand : Command {
    private Player player;
    private Tile tile;

    public PlayerReachCommand(Player player, Tile tile) : base() {
        this.player = player;
        this.tile = tile;
    }

    public override IEnumerator PerformCommand(Game game) {
        this.player.HasReached = true;
        this.player.IppatsuPotential = true;
        this.tile.UsedForReach = true;
        yield break;
    }
}
