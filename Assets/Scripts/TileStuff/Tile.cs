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

        if (tiles.Count == 0) {
            return sets;
        }

        tiles.Sort(Tile.CompareTiles);

        int counter = 1;
        Tile curTile = null;
        for (int i = 0; i < tiles.Count; ++i) {
            if (curTile == null) {
                curTile = tiles[i];
                counter = 1;
            }

            Tile tile = tiles[i];
            if (tile != curTile) {
                if (curTile.IsSame(tile)) {
                    ++counter;
                } else {
                    counter = 1;
                    curTile = tile;
                }
            }

            if (counter == groupNum) {
                sets.Add(curTile);
                curTile = null;
            }
        }

        return sets;
    }

    public static int GetHighestNumOfSameTilesOfType(List<Tile> _tiles, Tile.TileType type) {
        List<Tile> tiles = new List<Tile>(_tiles);

        List<Tile> typeTiles = tiles.FindAll(t => t.type == type);

        int count = CombatSceneController.MaxPlayerHandSize;

        for(int g = CombatSceneController.MaxPlayerHandSize; g > 0; --g, --count) {
            List<Tile> sets = Tile.ReturnGroupedTiles(typeTiles, g);

            if (sets.Count > 0) {
                return count;
            }
        }

        return count;
    }
    
    public static int GetNumberOfTilesToCompleteHand(List<Tile> _tiles) {
        int tilesNeeded = 0;
        int numNeededSetsLeft = CombatSceneController.SetSize;
        List<Tile> tiles = new List<Tile>(_tiles);

        List<Tile> sets = Tile.ReturnGroupedTiles(tiles, 3);
        numNeededSetsLeft -= sets.Count;
        //Debug.Log("set num " + sets.Count);

        if (numNeededSetsLeft <= 0) {
            return Math.Max(0, tilesNeeded);
        }

        // Remove all sets from searching tiles.
        foreach (Tile tile in sets) {
            for (int i = 0; i < 3; ++i) {
                tiles.Remove(tiles.Find(t => t.IsSame(tile)));
            }
        }

        List<Tile> pairs = Tile.ReturnGroupedTiles(tiles, 2);

        //Debug.Log("pair num " + pairs.Count);
        tilesNeeded += Math.Min(numNeededSetsLeft, pairs.Count) * 1;
        numNeededSetsLeft -= pairs.Count;

        if (numNeededSetsLeft <= 0) {
            return Math.Max(0, tilesNeeded);
        }

        // Remove all pairs from searching tiles.
        foreach (Tile tile in pairs) {
            for (int i = 0; i < 2; ++i) {
                tiles.Remove(tiles.Find(t => t.IsSame(tile)));
            }
        }

        List<Tile> singles = Tile.ReturnGroupedTiles(tiles, 1);
        //Debug.Log("singles num " + singles.Count);
        tilesNeeded += Math.Min(numNeededSetsLeft, singles.Count) * 2;
        numNeededSetsLeft -= singles.Count;

        //Debug.Log("sets left " + numNeededSetsLeft);

        if (numNeededSetsLeft > 0) {
            tilesNeeded += numNeededSetsLeft * 3;
        }

        return tilesNeeded;
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
