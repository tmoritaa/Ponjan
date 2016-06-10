using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public abstract class SkillCardInfo : CardInfo {
    public SkillCardInfo(string name, Cost cost) : base(CardType.Skill, name, cost) { }

    public override IEnumerator Cast(Game game, Card card) {
        game.EnqueueCommand(new PlayCardCommand(card, null));
        yield break;
    }
}
