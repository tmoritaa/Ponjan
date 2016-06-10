using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DrawCommand : Command {
    Player player;
    public DrawCommand(Player player) : base() {
        this.player = player;
    }

    public override IEnumerator PerformCommand(Game game) {
        this.player.Draw(1, game.Deck);
        yield break;
    }
}
