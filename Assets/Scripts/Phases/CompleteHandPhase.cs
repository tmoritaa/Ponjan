using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CompleteHandPhase : PhaseNode {
    public CompleteHandPhase() : base(PhaseID.CompleteHand) { }

    public override IEnumerator PerformPhase(Game game) {
        Player activePlayer = game.Players.Find(p => p.IsActive);

        List<HandCombination> validCombs = game.ReturnValidCombinations(new List<Tile>(activePlayer.HandZone.Tiles));

        if (validCombs.Count > 0) {
            Decision decision = new CompleteHandDecision(activePlayer, game);
            game.EnqueueDecision(decision);
            yield return 0;

            string resp = (string)decision.Response[0];

            switch(resp) {
                case "Complete":
                    validCombs.ForEach(h => UnityEngine.Debug.Log(h.Name + " " + h.Score));
                    game.GameComplete = true;
                    break;
                default:
                    break;
            }
        }

        yield break;
    }
}
