using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Tile {
    public enum TileType {
        Blue,
        Yellow,
        Red,
        White,
        Dragon,
    }
    private TileType type;
    public TileType Type {
        get { return this.type; }
    }

    private int id;
    public int Id {
        get { return this.id; }
    }

    private Player owner;
    public Player Owner {
        get { return this.owner; }
        set { this.owner = value; }
    }

    Zone zone;
    public Zone Zone {
        get { return this.zone; }
        set { this.zone = value; }
    }

    private int deckIdx = 0;
    public int DeckIdx {
        get { return this.deckIdx; }
        set { this.deckIdx = value; }
    }

    public Tile(TileType type, int id) {
        this.type = type;
        this.id = id;
    }
}
