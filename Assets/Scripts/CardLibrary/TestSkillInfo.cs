using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class TestSkillInfo : SkillCardInfo {
    private Card card;
    private BoardSquareZone moveZone = null;

    public TestSkillInfo() : base("TestSkill", new Cost(new List<Cost.CostEntry>() { new Cost.CostEntry(Cost.CostType.Material, 2) })) { }

    public override IEnumerator Cast(Game game, Card card) {
        Decision decision = new PickSquareForMovementDecision(card.Owner, game);
        game.EnqueueDecision(decision);
        yield return 0;

        this.card = card;
        this.moveZone = (BoardSquareZone)decision.Response[0];

        game.EnqueueCommand(new PlayCardCommand(this.card, null));
        
        yield return 0;

        yield break;   
    }

    public override void PerformCastEffect(Game game) {
        game.EnqueueCommand(new MovePlayerCommand(this.card.Owner, this.moveZone));
        this.card = null;
        this.moveZone = null;
        return;
    }
}
