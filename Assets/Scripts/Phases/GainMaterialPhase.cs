using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class GainMaterialPhase : PhaseNode{
    public GainMaterialPhase() : base(PhaseNode.PhaseID.GainMaterial, "Gain Material Phase") {}

    public override IEnumerator PerformPhase(Game game) {
        game.Players.ForEach(p => game.EnqueueCommand(new AddMaterialCommand(p)));
        
        yield break;
    }
}
