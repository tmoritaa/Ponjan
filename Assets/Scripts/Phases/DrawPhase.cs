using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class DrawPhase : PhaseNode{
    public DrawPhase() : base(PhaseNode.PhaseID.Draw, "Draw Phase") {}

    public override IEnumerator PerformPhase(Game game) {
        game.Players.ForEach(p => game.EnqueueCommand(new DrawCardCommand(p)));

        yield break;
    }
}
