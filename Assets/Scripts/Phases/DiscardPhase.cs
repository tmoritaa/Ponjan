using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DiscardPhase : PhaseNode {
    public DiscardPhase() : base(PhaseID.Discard) {}

    public override IEnumerator PerformPhase(Game game) {
        Player activePlayer = game.Players.Find(p => p.IsActive);

        Decision decision = new PickTileDecision(activePlayer, game);
        game.EnqueueDecision(decision);
        yield return 0;

        Tile tile = (Tile)decision.Response[0];
        game.EnqueueCommand(new DiscardCommand(activePlayer, tile));
        yield return 0;

        yield break;
    }

}
