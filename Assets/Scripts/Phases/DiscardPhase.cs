using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DiscardPhase : PhaseNode {
    public DiscardPhase() : base(PhaseNode.PhaseID.Discard, "Discard Phase") {}

    public override IEnumerator PerformPhase(Game game) {
        yield break;
    }
}
