using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DiscardPhase : PhaseNode {
    public DiscardPhase() : base(PhaseID.Discard) {}

    public override IEnumerator PerformPhase(Game game) {
        Player activePlayer = game.Players.Find(p => p.IsActive);

        Decision decision = new PickTileOrReachDecision(activePlayer, game);
        game.EnqueueDecision(decision);
        yield return 0;

        string action = (string)decision.Response[0];

        if (action.Equals("Discard")) {
            Tile tile = (Tile)decision.Response[1];
            game.EnqueueCommand(new DiscardCommand(activePlayer, tile));
        } else if (action.Equals("Reach")) {
            game.EnqueueCommand(new DiscardCommand(activePlayer, activePlayer.GetOddOneOutTile()));
            game.EnqueueCommand(new PlayerReachCommand(activePlayer));
        }

        yield return 0;

        yield break;
    }

}
