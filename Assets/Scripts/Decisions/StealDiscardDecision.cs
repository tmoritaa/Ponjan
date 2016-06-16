using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class StealDiscardDecision : Decision {
    public StealDiscardDecision(Player controller, Game game) : base(controller, game) { }

    public override bool HandleUIResponse(object response) {
        List<object> finalResponse = new List<object>();

        if (this.CanBeCastTo(response, typeof(string))) {
            string action = (string)response;
            if (action.Equals("Steal") || action.Equals("Cancel")) {
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
        this.game.EnqueueUIResponseRequest(new UIResponseRequest(UIResponseRequest.ResponseType.DecideToSteal));
        yield break;
    }

    public override IEnumerator HandleAI() {
        throw new NotImplementedException();
    }
}
