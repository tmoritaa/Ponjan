using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class AttackWithUnitCommand : Command {
    private Card card;

    public AttackWithUnitCommand(Card card) {
        this.card = card;
    }

    public override IEnumerator PerformCommand(Game game) {
        Debug.Assert(this.card.Zone.Type == Zone.ZoneType.Gameboard, "Unit trying to attack is not on game board!");

        // Get BoardSquareZones in attack range.
        HashSet<BoardSquareZone> zonesInRange = ((BoardSquareZone)this.card.Zone).GetSquaresInRange(card.Info.AttackRangeType, game.Board);

        // Perform deal damage command to each opponent card in range.
        foreach(BoardSquareZone zone in zonesInRange) {
            if (zone.Cards.Count == 1 && zone.Cards[0].Owner != this.card.Owner) {
                game.EnqueueCommand(new DealDamageToUnitCommand(zone.Cards[0], this.card.Info.AttackDamage));
            }

            int oppIdx = Math.Abs(this.card.Owner.Id - 1);
            if (zone.Players[oppIdx] != null) {
                game.EnqueueCommand(new PlayerTakeDamageCommand(zone.Players[oppIdx], this.card.Info.AttackDamage));
            }
        }

        yield break;
    }

}
