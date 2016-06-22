using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PickTileDecision : Decision {
    public PickTileDecision(Player player, Game game) : base(player, game) { }

    public override bool HandleUIResponse(object response) {
        List<object> finalResponse = new List<object>();

        if (this.CanBeCastTo(response, typeof(Tile))) {
            Tile tile = (Tile)response;
            if (tile.Owner == this.controller && tile.Zone.Type == Zone.ZoneType.Hand && 
                (!this.controller.HasReached || (this.controller.HasReached && this.controller.HandZone.Tiles.Last() == tile))) {
                finalResponse.Add("Discard");
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
        this.game.EnqueueUIResponseRequest(new UIResponseRequest(UIResponseRequest.ResponseType.SelectTile));
        yield break;
    }

    public override IEnumerator HandleAI() {
        this.HandleUIResponse(this.controller.AIPickTileToDiscard(game.HandCombinations, this.game));
        yield break;
    }
}