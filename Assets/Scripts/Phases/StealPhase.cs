using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class StealPhase : PhaseNode {
    public StealPhase() : base(PhaseID.Steal) { }

    public override IEnumerator PerformPhase(Game game) {
        Player activePlayer = game.Players.Find(p => p.IsActive);

        // Make list of players in steal priority order.
        List<Player> stealPriorityOrder = new List<Player>();
        int startIdx = (activePlayer.Id + 1) % game.Players.Count;
        for (int i = 0; i < game.Players.Count; ++i) {
            Player player = game.Players[(startIdx + i) % game.Players.Count];
            if (player != activePlayer) {
                stealPriorityOrder.Add(player);
            }
        }

        Tile stealableTile = activePlayer.DiscardZone.Tiles.Last();
        foreach (Player player in stealPriorityOrder) {
            // if conditions of stealing are available, ask for decision.
            if (player.CanStealTile(stealableTile)) {
                Decision decision = new StealDiscardDecision(player, game);
                game.EnqueueDecision(decision);
                yield return 0;

                string response = (string)decision.Response[0];

                if (response.Equals("Steal")) {
                    game.EnqueueCommand(new StealTileCommand(player, stealableTile));
                    game.EnqueueCommand(new ChangeActivePlayerCommand(activePlayer, player));
                    game.EnqueueCommand(new ChangeNextPhaseCommand(PhaseNode.PhaseID.CompleteHandWithSteal));
                    break;
                }
            }
        }

        yield break;
    }
}
