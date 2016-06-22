using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Tile {
    // TODO: Later make id and type of Tile class use this instead.
    public class TileProp {
        public Tile.TileType type;
        public int id;

        public TileProp(Tile tile) {
            this.type = tile.type;
            this.id = tile.id;
        }

        public TileProp(Tile.TileType type, int id) {
            this.type = type;
            this.id = id;
        }

        public override bool Equals(object obj) {
            if (obj == null) {
                return false;
            }

            TileProp prop = (TileProp)obj;

            if ((System.Object)prop == null) {
                return false;
            }

            return this.Equals(prop);
        }

        public bool Equals(Tile tile) {
            return this.Equals(new TileProp(tile));
        }

        public bool Equals(TileProp prop) {
            return (this.id == prop.id) && (this.type == prop.type);
        }

        public override int GetHashCode() {
            int hash = 13;
            hash = (hash * 7) + this.id.GetHashCode();
            hash = (hash * 7) + this.type.GetHashCode();
            return hash;
        }
    }

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
        if (tile == null) {
            return false;
        }

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

    public static float FindCompleteHandWithHighestProb(List<Tile> tiles, List<TileProp> usableTiles, Game game, out List<TileProp> outTilePropsUsed) {
        List<TileProp> tilePropsUsed = new List<TileProp>();
        List<Tile> sets;
        List<Tile> pairs;
        List<Tile> singles;

        Tile.SeparateIntoSetsPairsSingles(tiles, out sets, out pairs, out singles);

        int numberOfSetsNeeded = CombatSceneController.MaxPlayerHandSize / CombatSceneController.SetSize;

        // Handle prob for sets.
        float prob = 1.0f;
        sets.ForEach(t => tilePropsUsed.Add(new TileProp(t)));
        if (numberOfSetsNeeded <= sets.Count) {
            outTilePropsUsed = tilePropsUsed;
            return prob;
        }
        numberOfSetsNeeded -= sets.Count;

        // Handle prob for pairs.
        {
            List<TileDrawProb> probForEachPair = new List<TileDrawProb>();
            foreach (Tile tile in pairs) {
                probForEachPair.Add(game.GetProbOfDrawingTile(new TileProp(tile), 1));
            }

            probForEachPair.Sort(TileDrawProb.SortFunc);

            foreach (TileDrawProb drawProb in probForEachPair) {
                if (drawProb.prob <= 0) {
                    break;
                }

                prob *= drawProb.prob;

                tilePropsUsed.Add(drawProb.tileProp);

                numberOfSetsNeeded -= 1;
                if (numberOfSetsNeeded <= 0) {
                    outTilePropsUsed = tilePropsUsed;
                    return prob;
                }
            }
        }


        // Handle prob for singles.
        {
            List<TileDrawProb> probForEachSingle = new List<TileDrawProb>();
            foreach (Tile tile in singles) {
                probForEachSingle.Add(game.GetProbOfDrawingTile(new TileProp(tile), 2));
            }

            probForEachSingle.Sort(TileDrawProb.SortFunc);

            foreach (TileDrawProb drawProb in probForEachSingle) {
                if (drawProb.prob <= 0) {
                    break;
                }

                prob *= drawProb.prob;

                tilePropsUsed.Add(drawProb.tileProp);

                numberOfSetsNeeded -= 1;
                if (numberOfSetsNeeded <= 0) {
                    outTilePropsUsed = tilePropsUsed;
                    return prob;
                }
            }
        }

        // Handle for tiles not in hand.
        {
            List<TileDrawProb> probsForEachUsable = new List<TileDrawProb>();
            foreach (TileProp tileProp in usableTiles) {
                probsForEachUsable.Add(game.GetProbOfDrawingTile(tileProp, 3));
            }

            probsForEachUsable.Sort(TileDrawProb.SortFunc);

            foreach (TileDrawProb drawProb in probsForEachUsable) {
                if (drawProb.prob <= 0) {
                    break;
                }

                prob *= drawProb.prob;

                numberOfSetsNeeded -= 1;
                if (numberOfSetsNeeded <= 0) {
                    outTilePropsUsed = tilePropsUsed;
                    return prob;
                }
            }
        }

        // If gotten here, then probability is zero.
        outTilePropsUsed = tilePropsUsed;
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

    public static void SeparateIntoSetsPairsSingles(List<Tile> _tiles, out List<Tile> sets, out List<Tile> pairs, out List<Tile> singles) {
        List<Tile> tiles = new List<Tile>(_tiles);
        sets = Tile.ReturnGroupedTiles(tiles, 3);

        foreach (Tile tile in sets) {
            for (int i = 0; i < 3; ++i) {
                tiles.Remove(tiles.Find(t => t.IsSame(tile)));
            }
        }

        pairs = Tile.ReturnGroupedTiles(tiles, 2);

        foreach (Tile tile in pairs) {
            for (int i = 0; i < 2; ++i) {
                tiles.Remove(tiles.Find(t => t.IsSame(tile)));
            }
        }

        singles = tiles;
    }

    public static List<Tile> ReturnTilesWithoutDuplicates(List<Tile> tiles) {
        List<Tile> outList = new List<Tile>();
        foreach (Tile tile in tiles) {
            if (!outList.Exists(t => t.IsSame(tile))) {
                outList.Add(tile);
            }
        }

        return outList;
    }
    
    public static int GetNumberOfTilesToCompleteHand(List<Tile> _tiles) {
        int tilesNeeded = 0;
        int numNeededSetsLeft = CombatSceneController.SetSize;
        List<Tile> tiles = new List<Tile>(_tiles);

        List<Tile> sets;
        List<Tile> pairs;
        List<Tile> singles;

        Tile.SeparateIntoSetsPairsSingles(tiles, out sets, out pairs, out singles);

        numNeededSetsLeft -= sets.Count;
        if (numNeededSetsLeft <= 0) {
            return Math.Max(0, tilesNeeded);
        }

        tilesNeeded += Math.Min(numNeededSetsLeft, pairs.Count) * 1;
        numNeededSetsLeft -= pairs.Count;

        if (numNeededSetsLeft <= 0) {
            return Math.Max(0, tilesNeeded);
        }

        tilesNeeded += Math.Min(numNeededSetsLeft, singles.Count) * 2;
        numNeededSetsLeft -= singles.Count;

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
