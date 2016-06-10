using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PickSquareForMovementDecision : Decision {
    public PickSquareForMovementDecision(Player controller, Game game) : base(controller, game) {}

    public override bool HandleUIResponse(object response) {
        List<object> finalResponse = new List<object>();

        if (this.CanBeCastTo(response, typeof(BoardSquareZone))) {
            BoardSquareZone zone = (BoardSquareZone)response;
            if (this.controller.CurLocation.ZoneIsInRange(zone, BoardSquareZone.RangeType.Adjacent, game.Board)) {
                finalResponse.Add(zone);                
            }
        }


        bool valid = finalResponse.Count > 0;
        if (valid) {
            this.SaveCompleteUIResponse(finalResponse);
        }

        return valid;
    }

    public override IEnumerator HandleAI() {
        throw new NotImplementedException();
    }

    public override IEnumerator HandlePlayer() {
        this.game.EnqueueUIResponseRequest(new UIResponseRequest(UIResponseRequest.ResponseType.PickZone));
        yield break;
    }
}
