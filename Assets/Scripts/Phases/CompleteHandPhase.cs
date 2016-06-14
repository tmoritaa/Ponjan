using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CompleteHandPhase : PhaseNode {
    public CompleteHandPhase() : base(PhaseID.CompleteHand) { }

    public override IEnumerator PerformPhase(Game game) {
        // Check whether hand has 3 sets.
        // Next check if some kind of combination exists.

        yield break;
    }
}
