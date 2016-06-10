using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class AttackCyclePhase : PhaseNode{
    public AttackCyclePhase() : base(PhaseNode.PhaseID.AttackCycle, "Attack Cycle Phase") {}

    public override IEnumerator PerformPhase(Game game) {
        foreach(BoardSquareZone zone in game.Board.Squares) {
            if (zone.Cards.Count != 0 && zone.Cards[0].Info.Type == CardInfo.CardType.Unit) {
                game.EnqueueCommand(new IncreaseAttackCycleOfUnitCommand(zone.Cards[0], 1));
            }
        }

        yield return 0;

        game.EnqueueUIUpdateRequest(new UIUpdateRequest(UIUpdateRequest.UpdateType.UpdateBoard));

        yield return 0;

        yield break;
    }
}
