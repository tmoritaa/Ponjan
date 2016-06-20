using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PickTileOrReachDecision : PickTileDecision {
    public PickTileOrReachDecision(Player controller, Game game) : base(controller, game) { }

    public override bool HandleUIResponse(object response) {
        bool valid = base.HandleUIResponse(response);

        if (!valid) {
            List<object> finalResponse = new List<object>();

            if (this.CanBeCastTo(response, typeof(string))) {
                string action = (string)response;
                if (action.Equals("Reach")) {
                    finalResponse.Add(response);
                }
            }

            valid = finalResponse.Count > 0;
            if (valid) {
                this.SaveCompleteUIResponse(finalResponse);
            }
        }

        return valid;
    }

    public override IEnumerator HandlePlayer() {
        UIResponseRequest.ResponseType responseType = UIResponseRequest.ResponseType.SelectTileOrReach;
        if (this.controller.HasReached || !this.controller.OneAwayFromCompletion()) {
            responseType = UIResponseRequest.ResponseType.SelectTile;
        }
        this.game.EnqueueUIResponseRequest(new UIResponseRequest(responseType));
        yield break;
    }

    public override IEnumerator HandleAI() {
        if (!this.controller.HasReached && this.controller.OneAwayFromCompletion()) {
            this.HandleUIResponse("Reach");
        } else {
            IEnumerator enumerator = base.HandleAI();
            bool hasNext = true;
            do {
                hasNext = enumerator.MoveNext();
                yield return 0;
            } while (hasNext);
        }
        
        yield break;
    }
}
