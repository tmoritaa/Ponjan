using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DestroyCardCommand : Command {
    private Card card = null;

    public DestroyCardCommand(Card card) {
        this.card = card;
    }

    public override IEnumerator PerformCommand(Game game) {
        Player player = card.Owner;
        player.DestroyCard(this.card);
        yield break;
    }
}