using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PayMaterialCommand : Command {
    Player player;
    int val;

    public PayMaterialCommand(Player player, int val) : base() {
        this.player = player;
        this.val = val;
    }

    public override IEnumerator PerformCommand(Game game) {
        this.player.PayCost(this.val);
        yield break;
    }

}