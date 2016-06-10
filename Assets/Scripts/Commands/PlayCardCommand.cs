using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PlayCardCommand : Command {
    private Card card;
    private BoardSquareZone boardSquare;

    public PlayCardCommand(Card card, BoardSquareZone boardSquare) {
        this.card = card;
        this.boardSquare = boardSquare;
    }

    public override IEnumerator PerformCommand(Game game) {
        IEnumerator costEnum = this.card.Info.Cost.PayCost(game, this.card);
        bool hasNext = true;
        do {
            hasNext = costEnum.MoveNext();
            yield return 0;
        } while (hasNext);

        switch (this.card.Info.Type) {
            case CardInfo.CardType.Unit:
                this.card.Owner.PlayUnitCardFromHand(this.card, this.boardSquare);
                break;
            case CardInfo.CardType.Skill:
                this.card.Owner.PlaySkillCardFromHand(this.card);
                break;
        }

        this.card.Info.PerformCastEffect(game);

        yield break;
    }
}