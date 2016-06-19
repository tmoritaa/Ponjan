using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DisplayNoDeckScoringDecision : Decision {
    public DisplayNoDeckScoringDecision(Game game) : base(null, game) {}

    public override bool HandleUIResponse(object response) {
        return true;
    }

    public void HandleRequestSending() {
        UIResponseRequest request = new UIResponseRequest(UIResponseRequest.ResponseType.DisplayNoDeckScoring);
        game.EnqueueUIResponseRequest(request);
    }

    public override IEnumerator HandlePlayer() {
        this.HandleRequestSending();
        yield break;
    }

    public override IEnumerator HandleAI() {
        this.HandleRequestSending();
        yield break;
    }
}

