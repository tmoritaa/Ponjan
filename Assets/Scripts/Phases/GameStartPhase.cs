using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class GameStartPhase : PhaseNode {
    public GameStartPhase() : base(PhaseID.StartGame) { }

    public override IEnumerator PerformPhase(Game game) {
        List<Player> players = game.Players;

        for (int i = 0; i < 2; ++i) {
            foreach (Player player in players) {
                player.Draw(4, game.Deck);
            }
        }

        // TODO: should be random.
        players[0].IsBoss = true;
        players[0].IsActive = true;

        yield break;
    }

}
