using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PlayerTakeDamageCommand : Command {
    Player player;
    int val;

    public PlayerTakeDamageCommand(Player player, int val) : base() {
        this.player = player;
        this.val = val;
    }

    public override IEnumerator PerformCommand(Game game) {
        this.player.TakeDamage(this.val);
        yield break;
    }

}
