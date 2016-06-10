using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public abstract class UnitCardInfo : CardInfo {
    private Card card;
    private BoardSquareZone zone;

    public UnitCardInfo(string name, Cost cost) : base(CardType.Unit, name, cost) { }

    public override IEnumerator Cast(Game game, Card card) {
        BoardSquareZone square = card.Owner.CurLocation;

        if (square.Cards.Count > 0) {
            game.EnqueueCommand(new DestroyCardCommand(square.Cards[0]));
        }
        game.EnqueueCommand(new PlayCardCommand(card, square));

        yield break;
    }

    public override void PerformCastEffect(Game game) {
        return;
    }
}
