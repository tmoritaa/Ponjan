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

    public static int CompareTiles(Tile x, Tile y) {
        if (x.type != y.type) {
            return (x.type < y.type) ? -1 : 1;
        } else if (x.id != y.id ){
            return (x.id < y.id) ? -1 : 1;
        }
        return 0;
    }

    public Tile(TileType type, int id) {
        this.type = type;
        this.id = id;
    }
}
