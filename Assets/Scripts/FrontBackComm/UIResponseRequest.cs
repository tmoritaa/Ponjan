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
    };

    public ResponseType requestType = ResponseType.None;

    public UIResponseRequest(ResponseType requestType) {
        this.requestType = requestType;
    }
}
