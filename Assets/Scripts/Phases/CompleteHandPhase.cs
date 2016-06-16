﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CompleteHandPhase : PhaseNode {
    public CompleteHandPhase() : base(PhaseID.CompleteHand) {}

    public override IEnumerator PerformPhase(Game game) {
        Player activePlayer = game.Players.Find(p => p.IsActive);

        List<Tile> allTiles = new List<Tile>(activePlayer.HandZone.Tiles);
        allTiles.AddRange(activePlayer.StealZone.Tiles);
        allTiles.Sort(Tile.CompareTiles);

        List<HandCombination> validCombs = game.ReturnValidCombinations(allTiles);

        if (validCombs.Count > 0) {
            Decision decision = new CompleteHandDecision(activePlayer, game, CompleteHandDecision.CompletionType.Draw);
            game.EnqueueDecision(decision);
            yield return 0;

            string resp = (string)decision.Response[0];

            if (resp.Equals("Complete")) {
                validCombs.ForEach(h => UnityEngine.Debug.Log("Draw " + h.Name + " " + h.Score));
                game.GameComplete = true;
            }
        }

        yield break;
    }
}
