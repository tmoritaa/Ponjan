using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ChangeActivePlayerPhase : PhaseNode {
    public ChangeActivePlayerPhase() : base(PhaseID.ChangeActivePlayer) {}

    public override IEnumerator PerformPhase(Game game) {
        if (game.Deck.Tiles.Count <= 0) {
            List<int> scoreChangePerPlayer = Game.CalculateScoreForNoDeck(game.Players);
            game.Players.ForEach(p => p.Score += scoreChangePerPlayer[p.Id]);
            game.EnqueueDecision(new DisplayNoDeckScoringDecision(game));
            yield return 0;

            game.GameComplete = true;
        } else {
            List<Player> players = game.Players;
            Player curActivePlayer = players.Find(p => p.IsActive);
            Player nextActivePlayer = players[(curActivePlayer.Id + 1) % players.Count];
            game.EnqueueCommand(new ChangeActivePlayerCommand(curActivePlayer, nextActivePlayer));
        }
        
        yield break;
    }
}
