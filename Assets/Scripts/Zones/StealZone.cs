using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class StealZone : Zone {
    public StealZone() : base(Zone.ZoneType.Steal) {}

    public Player GetLastStolenPlayer() {
        Player player = null;
        for (int i = this.tiles.Count - 1; i >= 0; --i) {
            if (this.tiles[i].StolenPlayer != null) {
                player = this.tiles[i].StolenPlayer;
                break;
            }
        }

        UnityEngine.Debug.Assert(player != null, "This should never be null.");

        return player;
    }
}
