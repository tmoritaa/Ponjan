using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Cost {
    public enum CostType {
        Material,
        Life,
    }

    public class CostEntry {
        public CostType costType;
        public int value;

        public CostEntry(CostType costType, int value) {
            this.costType = costType;
            this.value = value;
        }
    }

    List<CostEntry> costs;

    public Cost(List<CostEntry> costs) {
        this.costs = costs;
    }

    public bool IsPayable(Card card) {
        bool playable = true;
        foreach(CostEntry entry in this.costs) {
            switch(entry.costType) {
                case CostType.Material:
                    playable = card.Owner.CurMaterial >= entry.value;
                    break;
                case CostType.Life:
                    playable = card.Owner.CurHealth >= entry.value;
                    break;
            }

            if (!playable) {
                break;
            }
        }

        return playable;
    }

    public IEnumerator PayCost(Game game, Card card) {
        foreach(CostEntry entry in this.costs) {
            switch(entry.costType) {
                case CostType.Material:
                    game.EnqueueCommand(new PayMaterialCommand(card.Owner, entry.value));
                    break;
                case CostType.Life:
                    game.EnqueueCommand(new PlayerTakeDamageCommand(card.Owner, entry.value));
                    break;
            }
        }

        yield break;
    }
}
