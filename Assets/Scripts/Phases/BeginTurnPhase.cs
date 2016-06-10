using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class BeginTurnPhase : PhaseNode {
    public BeginTurnPhase() : base(PhaseID.BeginTurn, "Begin Turn Phase") {}

    public override IEnumerator PerformPhase(Game game) {
        yield break;
    }
}