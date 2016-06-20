using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class GameStartPhase : PhaseNode {
    public GameStartPhase() : base(PhaseID.StartGame) { }

    public override IEnumerator PerformPhase(Game game) {
        List<Player> players = game.Players;

        if (game.CurRound == 1) {
            int playerIdx = UnityEngine.Random.Range(0, players.Count);
            players[playerIdx].IsBoss = true;
            players[playerIdx].IsActive = true;
        } else {
            Player prevBossPlayer = players.Find(p => p.IsBoss);
            prevBossPlayer.IsBoss = false;

            int nextBossPlayerIdx = (prevBossPlayer.Id + 1) % players.Count;
            players[nextBossPlayerIdx].IsBoss = true;
            players[nextBossPlayerIdx].IsActive = true;

        }

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
