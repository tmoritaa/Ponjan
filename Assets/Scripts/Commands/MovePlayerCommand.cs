using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class MovePlayerCommand : Command {
    private Player player;
    private BoardSquareZone zone;

    public MovePlayerCommand(Player player, BoardSquareZone zone) {
        this.player = player;
        this.zone = zone;
    }

    public override IEnumerator PerformCommand(Game game) {
        this.player.MoveToSquare(zone);
        yield break;
    }
}
