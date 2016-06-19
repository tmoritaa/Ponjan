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

    private Player stolenPlayer = null;
    public Player StolenPlayer {
        get { return this.stolenPlayer; }
        set { this.stolenPlayer = value; }
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

    public void Reset() {
        this.stolenPlayer = null;
    }

    public bool IsSame(Tile tile) {
        return (this.type == tile.type && this.id == tile.id);
    }

    public static int CompareTiles(Tile x, Tile y) {
        if (x.type != y.type) {
            return (x.type < y.type) ? -1 : 1;
        } else if (x.id != y.id ){
            return (x.id < y.id) ? -1 : 1;
        }
        return 0;
    }

    public static List<Tile> ReturnGroupedTiles(List<Tile> _tiles, int groupNum = CombatSceneController.SetSize) {
        List<Tile> tiles = new List<Tile>(_tiles);
        List<Tile> sets = new List<Tile>();

        tiles.Sort(Tile.CompareTiles);

        int counter = 1;
        Tile curTile = tiles[0];
        for (int i = 1; i < tiles.Count; ++i) {
            if (curTile == null) {
                curTile = tiles[i];
                counter = 1;
                continue;
            }

            Tile tile = tiles[i];
            if (curTile.IsSame(tile)) {
                ++counter;
            } else {
                counter = 1;
                curTile = tile;
            }

            if (counter == groupNum) {
                sets.Add(curTile);
                curTile = null;
            }
        }

        return sets;
    } 

    public Tile(TileType type, int id) {
        this.type = type;
        this.id = id;
    }

    public Tile(Tile tile) {
        this.type = tile.type;
        this.id = tile.id;
        this.owner = tile.owner;
        this.stolenPlayer = tile.stolenPlayer;
        this.zone = tile.zone;
        this.deckIdx = tile.deckIdx;    
    }
}
