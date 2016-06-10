using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DeckZone : Zone {
    public DeckZone() : base(Zone.ZoneType.Deck) { }

    public Tile Draw() {
        Tile tile = this.tiles.Last();
        this.tiles.RemoveAt(this.tiles.Count - 1);
        return tile;
    }
}
