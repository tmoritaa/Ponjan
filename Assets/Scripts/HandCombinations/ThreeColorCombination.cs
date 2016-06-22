using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ThreeColorCombination : HandCombination {
    public ThreeColorCombination() : base("Three Colors", 2) { }

    public override bool HandHasCombination(List<Tile> tiles, HandCombination.CompletionType compType) {
        List<Tile> sets = Tile.ReturnGroupedTiles(tiles);

        return (sets.Count == 3 && 
            sets.Exists(t => t.Type == Tile.TileType.Red) && 
            sets.Exists(t => t.Type == Tile.TileType.Blue) && 
            sets.Exists(t => t.Type == Tile.TileType.Yellow));
    }

    public override float GetProbabilityOfCompletion(List<Tile> _tiles, List<Tile.TileProp> allTileData, Game game, out List<Tile.TileProp> outTilePropsUsed) {
        List<Tile> tiles = new List<Tile>(_tiles);

        Tile.TileType[] tileTypes = new Tile.TileType[3] { Tile.TileType.Red, Tile.TileType.Blue, Tile.TileType.Yellow };

        List<Tile.TileProp> usableTiles = new List<Tile.TileProp>();
        List<Tile> tilesForHand = new List<Tile>();
        foreach(Tile.TileType tileType in tileTypes) {
            List<Tile> sets;
            List<Tile> pairs;
            List<Tile> singles;

            List<Tile> colorTiles = tiles.FindAll(t => t.Type == tileType);
            Tile.SeparateIntoSetsPairsSingles(colorTiles, out sets, out pairs, out singles);

            if (sets.Count > 0) {
                for (int i = 0; i < 3; ++i) {
                    Tile tile = tiles.Find(t => t.IsSame(sets[0]));
                    tilesForHand.Add(tile);
                    tiles.Remove(tile);
                }
            } else if (pairs.Count > 0) {
                TileDrawProb bestDrawProb = new TileDrawProb(new Tile.TileProp(pairs[0]), 0);
                foreach (Tile tile in pairs) {
                    TileDrawProb drawProb = game.GetProbOfDrawingTile(new Tile.TileProp(tile), 1);

                    if (drawProb.prob > bestDrawProb.prob) {
                        bestDrawProb = drawProb;
                    }
                }

                for (int i = 0; i < 2; ++i) {
                    Tile tile = tiles.Find(t => bestDrawProb.tileProp.Equals(t));
                    tilesForHand.Add(tile);
                    tiles.Remove(tile);
                }
            } else if (singles.Count > 0) {
                TileDrawProb bestDrawProb = new TileDrawProb(new Tile.TileProp(singles[0]), 0);
                foreach (Tile tile in singles) {
                    TileDrawProb drawProb = game.GetProbOfDrawingTile(new Tile.TileProp(tile), 2);

                    if (drawProb.prob > bestDrawProb.prob) {
                        bestDrawProb = drawProb;
                    }
                }

                Tile tileToAdd = tiles.Find(t => bestDrawProb.tileProp.Equals(t));
                tilesForHand.Add(tileToAdd);
                tiles.Remove(tileToAdd);
            } else {
                usableTiles.AddRange(allTileData.FindAll(t => t.type == tileType));
            }
        }

        List<Tile.TileProp> tilePropsUsed;
        float prob = Tile.FindCompleteHandWithHighestProb(tilesForHand, usableTiles, game, out tilePropsUsed);

        // Remove any used tiles that have excess numbers.
        List<Tile.TileProp> tobeRemoved = new List<Tile.TileProp>();
        foreach(Tile.TileProp tileProp in tilePropsUsed) {
            if (_tiles.FindAll(t => tileProp.Equals(t)).Count > CombatSceneController.SetSize) {
                tobeRemoved.Add(tileProp);
            }
        }
        tobeRemoved.ForEach(tp => tilePropsUsed.Remove(tp));

        outTilePropsUsed = tilePropsUsed;
        return prob;
    }
}
