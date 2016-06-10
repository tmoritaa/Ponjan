using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CompleteHandPhase : PhaseNode {
    public CompleteHandPhase() : base(PhaseID.CompleteHand) { }

    public override IEnumerator PerformPhase(Game game) {
        yield break;
    }
}
