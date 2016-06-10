using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class MovementPhase : PhaseNode {
    public MovementPhase() : base(PhaseID.Movement, "Movement Phase") {}

    public override IEnumerator PerformPhase(Game game) {
        List<Player> playerOrder = new List<Player>();
        playerOrder.Add(game.Players.Find(p => p.HasInitiative));
        playerOrder.Add(game.Players.Find(p => !p.HasInitiative));

        foreach(Player activePlayer in playerOrder) {
            Decision decision = new PickSquareForMovementDecision(activePlayer, game);
            game.EnqueueDecision(decision);
            yield return 0;

            BoardSquareZone zone = (BoardSquareZone)decision.Response[0];
            game.EnqueueCommand(new MovePlayerCommand(activePlayer, zone));
            yield return 0;

            game.EnqueueUIUpdateRequest(new UIUpdateRequest(UIUpdateRequest.UpdateType.UpdateBoard));
            yield return 0;
        }

        yield break;
    }

}
