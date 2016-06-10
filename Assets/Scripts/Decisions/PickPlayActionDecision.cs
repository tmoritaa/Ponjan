using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PickPlayActionDecision : Decision {
    public PickPlayActionDecision(Player controller, Game game) : base(controller, game) { }

    public override bool HandleUIResponse(object response) {
        List<object> finalResponse = new List<object>();

        if (this.CanBeCastTo(response, typeof(string))) {
            string str = (string)response;
            if (str.Equals("Pass")) {
                finalResponse.Add("Pass");
            }
        } else if (this.CanBeCastTo(response, typeof(Card))) {
            Card card = (Card)response;
            bool canBePlayed = false;

            switch(card.Info.Type) {
                case CardInfo.CardType.Unit:
                    canBePlayed = card.Zone.Type == Zone.ZoneType.Hand && card.Owner == this.controller && this.controller.CurLocation.CanBePlayedOn(this.controller) && card.CanPayCost();
                    break;
                case CardInfo.CardType.Skill:
                    canBePlayed = card.Zone.Type == Zone.ZoneType.Hand && card.Owner == this.controller && card.CanPayCost();
                    break;
            }

            if (canBePlayed) {
                finalResponse.Add("Play");
                finalResponse.Add(card);
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
        this.game.EnqueueUIResponseRequest(new UIResponseRequest(UIResponseRequest.ResponseType.PlayPhaseAction));
        yield break;
    }
}
