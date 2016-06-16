using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ChangeNextPhaseCommand : Command {
    PhaseNode.PhaseID nextPhase;
    public ChangeNextPhaseCommand(PhaseNode.PhaseID phaseId) : base() {
        this.nextPhase = phaseId;
    }

    public override IEnumerator PerformCommand(Game game) {
        game.ForcePhaseAsNext(this.nextPhase);
        yield break;
    }
}
