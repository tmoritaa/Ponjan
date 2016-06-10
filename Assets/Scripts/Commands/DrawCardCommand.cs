using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DrawCardCommand : Command {
    private Player player;

    public DrawCardCommand(Player player) {
        this.player = player;
    }

    public override IEnumerator PerformCommand(Game game) {
        this.player.Draw();
        yield break;
    }
}