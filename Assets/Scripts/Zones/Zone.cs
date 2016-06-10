using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public abstract class Zone {
    public enum ZoneType {
        Hand,
        Deck,
        Gameboard,
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

    public virtual void AddCard(Tile tile) {
        if (tile.Zone != null) {
            tile.Zone.RemoveCard(tile);
        }

        tile.Zone = this;
        this.tiles.Add(tile);
    }

    public virtual void RemoveCard(Tile tile) {
        this.tiles.Remove(tile);
        tile.Zone = null;
    }
}
