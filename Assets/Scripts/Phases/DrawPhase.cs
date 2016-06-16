using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DrawPhase : PhaseNode {
    public DrawPhase() : base(PhaseID.Draw) {}

    public override IEnumerator PerformPhase(Game game) {
        Player activePlayer = game.Players.Find(p => p.IsActive);
        game.EnqueueCommand(new DrawCommand(activePlayer));
        yield break;
    }
}
