using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class HandZone : Zone {
    public HandZone() : base(Zone.ZoneType.Hand) { }

    public void SortHand() {
        this.tiles.Sort(Tile.CompareTiles);
    }
}
