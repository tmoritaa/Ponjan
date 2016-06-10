using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ChangeActivePlayerCommand : Command {
    private Player curActivePlayer;
    private Player nextActivePlayer;

    public ChangeActivePlayerCommand(Player curActivePlayer, Player nextActivePlayer) {
        this.curActivePlayer = curActivePlayer;
        this.nextActivePlayer = nextActivePlayer;
    }

    public override IEnumerator PerformCommand(Game game) {
        this.curActivePlayer.IsActive = false;
        this.nextActivePlayer.IsActive = true;

        yield break;
    }
}

