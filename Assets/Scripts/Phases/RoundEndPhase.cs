using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class RoundEndPhase : PhaseNode { 
    public RoundEndPhase() : base(PhaseNode.PhaseID.EndRound) { }

    public override IEnumerator PerformPhase(Game game) {
        if (game.CurRound >= game.NumberOfRounds) {
            game.EnqueueUIUpdateRequest(new UIUpdateRequest(UIUpdateRequest.UpdateType.DisplayFinalResults));
        } else {
            List<Player> players = game.Players;

            Player prevBossPlayer = players.Find(p => p.IsBoss);

            if (!prevBossPlayer.ScoredThisRound) {
                prevBossPlayer.IsBoss = false;

                int nextBossPlayerIdx = (prevBossPlayer.Id + 1) % players.Count;
                players[nextBossPlayerIdx].IsBoss = true;

                game.CurRound += 1;
            }
        }

        yield break;
    }
}
