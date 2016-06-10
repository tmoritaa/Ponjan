using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DealDamageToUnitCommand : Command {
    private Card target;
    private int value;

    public DealDamageToUnitCommand(Card target, int value) {
        this.target = target;
        this.value = value;
    }

    public override IEnumerator PerformCommand(Game game) {
        this.target.DealDamage(value);

        if (this.target.ShouldBeDestroyed()) {
            game.EnqueueCommand(new DestroyCardCommand(this.target));
            yield return 0;
        }

        yield break;
    }
}
