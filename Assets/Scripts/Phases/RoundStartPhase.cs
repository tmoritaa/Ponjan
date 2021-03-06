﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class RoundStartPhase : PhaseNode {
    public RoundStartPhase() : base(PhaseID.StartRound) { }

    public override IEnumerator PerformPhase(Game game) {
        List<Player> players = game.Players;

        Player bossPlayer = players.Find(p => p.IsBoss);
        bossPlayer.IsActive = true;

        game.Deck.Shuffle();

        int diceRoll = UnityEngine.Random.Range(1, 7) + UnityEngine.Random.Range(1, 7);
        game.Deck.InitDeckIndicesForPlayers(diceRoll, game.Players);

        for (int i = 0; i < 2; ++i) {
            foreach (Player player in players) {
                player.Draw(4, game.Deck);
            }
        }

        foreach (Player player in players) {
            player.SortHand();
        }

        yield break;
    }

}
