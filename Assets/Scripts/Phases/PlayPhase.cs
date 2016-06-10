using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class PlayPhase : PhaseNode {
    public PlayPhase() : base(PhaseNode.PhaseID.Play, "Play Phase") {}

    public override IEnumerator PerformPhase(Game game) {
        Queue<Player> playerOrder = new Queue<Player>();
        playerOrder.Enqueue(game.Players.Find(p => p.HasInitiative));
        playerOrder.Enqueue(game.Players.Find(p => !p.HasInitiative));

        while (playerOrder.Count > 0) {
            Player activePlayer = playerOrder.Dequeue();

            Decision decision = new PickPlayActionDecision(activePlayer, game);
            game.EnqueueDecision(decision);
            yield return 0;

            string respStr = (string)decision.Response[0];

            switch (respStr) {
                case "Play":
                    {
                        Card card = (Card)decision.Response[1];

                        IEnumerator cardEnum = card.Info.Cast(game, card);
                        bool hasNext = true;
                        do {
                            hasNext = cardEnum.MoveNext();
                            yield return 0;
                        } while (hasNext);

                        yield return 0;
                    }
                    break;
            }

            if (!respStr.Equals("Pass")) {
                playerOrder.Enqueue(activePlayer);
            }

            game.EnqueueUIUpdateRequest(new UIUpdateRequest(UIUpdateRequest.UpdateType.UpdateBoard));
            yield return 0;
        }

        yield break;
    }
}
