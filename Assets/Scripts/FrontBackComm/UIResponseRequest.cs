using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UIResponseRequest {
    public enum ResponseType {
        None,
        SelectTile,
        SelectTileOrReach,
        DecideToComplete,
        DecideToSteal,
        DisplayCompletedHand,
        DisplayNoDeckScoring,
    };

    public ResponseType requestType = ResponseType.None;

    public List<object> objs = new List<object>();

    public UIResponseRequest(ResponseType requestType) {
        this.requestType = requestType;
    }
}
