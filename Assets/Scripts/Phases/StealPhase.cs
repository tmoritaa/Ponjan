﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class StealPhase : PhaseNode {
    public StealPhase() : base(PhaseID.Steal) { }

    private List<HandCombination> GetPotentialHandCompletions(Game game, Player player, Tile stolenTile) {
        List<Tile> allTiles = new List<Tile>(player.HandZone.Tiles);
        allTiles.AddRange(player.StealZone.Tiles);
        allTiles.Add(stolenTile);
        allTiles.Sort(Tile.CompareTiles);

        List<HandCombination> validCombs = game.ReturnValidCombinations(allTiles, HandCombination.CompletionType.Steal);

        return validCombs;
    }

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
                List<HandCombination> validCombs = this.GetPotentialHandCompletions(game, player, stealableTile);

                // If can complete hand, ask.
                if (validCombs.Count > 0) {
                    Decision decision = new CompleteHandDecision(activePlayer, game, HandCombination.CompletionType.Steal);
                    game.EnqueueDecision(decision);
                    yield return 0;

                    string resp = (string)decision.Response[0];

                    if (resp.Equals("Complete")) {
                        player.StealTileForCompletion(stealableTile, activePlayer);
                        player.ScorePoints(validCombs, new List<Player>() { activePlayer });
                        game.EnqueueDecision(new DisplayCompletedHandDecision(activePlayer, game, validCombs));
                        yield return 0;

                        game.GameComplete = true;
                    }
                // Otherwise, just ask if they want to steal.
                } else {
                    Decision decision = new StealDiscardDecision(player, game);
                    game.EnqueueDecision(decision);
                    yield return 0;

                    string response = (string)decision.Response[0];

                    if (response.Equals("Steal")) {
                        game.EnqueueCommand(new StealTileCommand(activePlayer, player, stealableTile));
                        game.EnqueueCommand(new ChangeActivePlayerCommand(activePlayer, player));
                        game.EnqueueCommand(new ChangeNextPhaseCommand(PhaseNode.PhaseID.Discard));
                        break;
                    }
                }
            }
        }

        yield break;
    }
}
