using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PlayerReachCommand : Command {
    Player player;

    public PlayerReachCommand(Player player) : base() {
        this.player = player;
    }

    public override IEnumerator PerformCommand(Game game) {
        this.player.HasReached = true;
        this.player.IppatsuPotential = true;
        yield break;
    }
}
