using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class AbilityPhase : PhaseNode {
    public AbilityPhase() : base(PhaseNode.PhaseID.Ability, "Ability Phase") {}

    public override IEnumerator PerformPhase(Game game) {
        yield break;
    }
}