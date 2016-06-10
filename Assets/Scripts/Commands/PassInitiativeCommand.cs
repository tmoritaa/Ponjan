using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PassInitiativeCommand : Command {
    private List<Player> players;

    public PassInitiativeCommand(List<Player> players) {
        this.players = players;
    }

    public override IEnumerator PerformCommand(Game game) {
        this.players.ForEach(p => p.HasInitiative = !p.HasInitiative);
        yield break;
    }
}
