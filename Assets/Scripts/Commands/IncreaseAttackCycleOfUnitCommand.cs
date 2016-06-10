using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class IncreaseAttackCycleOfUnitCommand : Command {
    private Card card;
    private int incVal;
    public IncreaseAttackCycleOfUnitCommand(Card card, int incVal) {
        this.card = card;
        this.incVal = incVal;
    }

    public override IEnumerator PerformCommand(Game game) {
        this.card.IncreaseCurAttackCycle(incVal);

        if (this.card.ReadyToAttack()) {
            game.EnqueueCommand(new AttackWithUnitCommand(this.card));
            this.card.ResetAttackCycle();
        }

        yield break;
    }
}
