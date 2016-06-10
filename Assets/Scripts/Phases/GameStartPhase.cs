using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class GameStartPhase : PhaseNode{
    public GameStartPhase() : base(PhaseNode.PhaseID.StartGame, "Game Start Phase") {}

    public override IEnumerator PerformPhase(Game game) {
        game.Players.ForEach(p => p.Draw(4));
        game.Players[0].HasInitiative = true;

        int playerOneStartIdx = CombatSceneController.Instance.BoardDimension / 2;
        int playerTwoStartIdx = (CombatSceneController.Instance.BoardDimension * CombatSceneController.Instance.BoardDimension - 1) - playerOneStartIdx;
        game.Players[0].MoveToSquare(game.Board.Squares[playerOneStartIdx]);
        game.Players[1].MoveToSquare(game.Board.Squares[playerTwoStartIdx]);
        game.EnqueueUIUpdateRequest(new UIUpdateRequest(UIUpdateRequest.UpdateType.UpdateBoard));
        yield break;
    }
}
