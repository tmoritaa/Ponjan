using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class AddMaterialCommand : Command {
    private Player player;

    public AddMaterialCommand(Player player) {
        this.player = player;
    }

    public override IEnumerator PerformCommand(Game game) {
        this.player.AddMaterial(1);
        yield break;
    }
}