using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public abstract class Zone {
    public enum ZoneType {
        Hand,
        Deck,
        Discard,
    }

    protected ZoneType type;
    public ZoneType Type {
        get { return this.type; }
    }

    protected List<Tile> tiles = new List<Tile>();
    public List<Tile> Tiles {
        get { return this.tiles; }
    }

    public Zone(ZoneType type) {
        this.type = type;
    }

    public virtual void AddTile(Tile tile) {
        if (tile.Zone != null) {
            tile.Zone.RemoveTile(tile);
        }

        tile.Zone = this;
        this.tiles.Add(tile);
    }

    public virtual void RemoveTile(Tile tile) {
        this.tiles.Remove(tile);
        tile.Zone = null;
    }
}
