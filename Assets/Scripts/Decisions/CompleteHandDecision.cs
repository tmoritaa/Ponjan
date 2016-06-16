using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CompleteHandDecision : Decision {
    public enum CompletionType {
        Draw,
        Steal,
    }
    private CompletionType compType;

    public CompleteHandDecision(Player player, Game game, CompletionType compType) : base(player, game) {
        this.compType = compType;
    }

    public override bool HandleUIResponse(object response) {
        List<object> finalResponse = new List<object>();

        if (this.CanBeCastTo(response, typeof(string))) {
            string action = (string)response;
            if (action.Equals("Complete") || 
                (((this.compType == CompletionType.Steal && this.controller.HandZone.Tiles.Count > 0) || this.compType == CompletionType.Draw) && action.Equals("Cancel"))) {
                finalResponse.Add(response);
            }
        }

        bool valid = finalResponse.Count > 0;
        if (valid) {
            this.SaveCompleteUIResponse(finalResponse);
        }

        return valid;
    }

    public override IEnumerator HandlePlayer() {
        this.game.EnqueueUIResponseRequest(new UIResponseRequest(UIResponseRequest.ResponseType.DecideToComplete));
        yield break;
    }

    public override IEnumerator HandleAI() {
        throw new NotImplementedException();
    }
}
