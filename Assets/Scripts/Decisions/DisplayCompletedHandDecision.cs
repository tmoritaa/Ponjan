using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DisplayCompletedHandDecision : Decision {
    List<HandCombination> validCombs;

    public DisplayCompletedHandDecision(Player controller, Game game, List<HandCombination> validCombs) : base(controller, game) {
        this.validCombs = validCombs;
    }

    public override bool HandleUIResponse(object response) {
        return true;
    }

    public override IEnumerator HandlePlayer() {
        UIResponseRequest request = new UIResponseRequest(UIResponseRequest.ResponseType.DisplayCompletedHand);
        request.objs.Add(this.controller);
        this.validCombs.ForEach(c => request.objs.Add(c));
        game.EnqueueUIResponseRequest(request);
        yield break;
    }

    public override IEnumerator HandleAI() {
        throw new NotImplementedException();
    }
}

