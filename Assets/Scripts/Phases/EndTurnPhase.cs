using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class EndTurnPhase : PhaseNode {
    public EndTurnPhase() : base(PhaseNode.PhaseID.EndTurn, "End Turn Phase") { }

    public override IEnumerator PerformPhase(Game game) {
        game.EnqueueCommand(new PassInitiativeCommand(game.Players));
        yield break;
    }
}