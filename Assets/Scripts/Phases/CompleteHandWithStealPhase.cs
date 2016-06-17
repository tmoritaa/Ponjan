using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CompleteHandWithStealPhase : PhaseNode {
    public CompleteHandWithStealPhase() : base(PhaseID.CompleteHandWithSteal) {}

    public override IEnumerator PerformPhase(Game game) {
        Player activePlayer = game.Players.Find(p => p.IsActive);

        List<Tile> allTiles = new List<Tile>(activePlayer.HandZone.Tiles);
        allTiles.AddRange(activePlayer.StealZone.Tiles);
        allTiles.Sort(Tile.CompareTiles);

        List<HandCombination> validCombs = game.ReturnValidCombinations(allTiles, HandCombination.CompletionType.Steal);

        if (validCombs.Count > 0) {
            Decision decision = new CompleteHandDecision(activePlayer, game, HandCombination.CompletionType.Steal);
            game.EnqueueDecision(decision);
            yield return 0;

            string resp = (string)decision.Response[0];

            if (resp.Equals("Complete")) {
                activePlayer.ScorePoints(validCombs, HandCombination.CompletionType.Steal, game.Players);
                game.EnqueueDecision(new DisplayCompletedHandDecision(activePlayer, game, validCombs));
                yield return 0;

                game.GameComplete = true;
            }
        }

        yield break;
    }
}
