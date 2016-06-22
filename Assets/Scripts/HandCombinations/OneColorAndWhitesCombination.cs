using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class OneColorAndWhitesCombination : HandCombination {
    public OneColorAndWhitesCombination() : base("One Color and Whites", 2) { }

    public override bool HandHasCombination(List<Tile> tiles, HandCombination.CompletionType compType) {
        List<Tile> sets = Tile.ReturnGroupedTiles(tiles);

        int redExists = sets.Exists(t => t.Type == Tile.TileType.Red) ? 1 : 0;
        int blueExists = sets.Exists(t => t.Type == Tile.TileType.Blue) ? 1 : 0;
        int yellowExists = sets.Exists(t => t.Type == Tile.TileType.Yellow) ? 1 : 0;
        bool whiteExists = sets.Exists(t => t.Type == Tile.TileType.White);
        bool dragonExists = sets.Exists(t => t.Type == Tile.TileType.Dragon);

        int numColors = redExists + blueExists + yellowExists;

        bool valid = sets.Count == 3 && numColors == 1 && (whiteExists || dragonExists);

        return valid;
    }

    public override float GetProbabilityOfCompletion(List<Tile> _tiles, List<Tile.TileProp> allTileData, Game game, out List<Tile.TileProp> outTilePropsUsed) {
        List<Tile> tiles = new List<Tile>(_tiles);

        float highestProb = 0;
        Tile.TileType[] tileTypes = new Tile.TileType[3] { Tile.TileType.Red, Tile.TileType.Blue, Tile.TileType.Yellow };

        List<Tile.TileProp> finalTilePropsUsed = new List<Tile.TileProp>();
        for (int i = 0; i < 3; ++i) {
            Tile.TileType type = tileTypes[i];

            List<Tile.TileProp> tilePropsUsed;
            float prob = Tile.FindCompleteHandWithHighestProb(
                tiles.FindAll(t => t.Type == type || t.Type == Tile.TileType.White || t.Type == Tile.TileType.Dragon), 
                allTileData.FindAll(t => t.type == type || t.type == Tile.TileType.White || t.type == Tile.TileType.Dragon), 
                game, out tilePropsUsed);

            if (prob >= highestProb) {
                finalTilePropsUsed = tilePropsUsed;
                highestProb = prob;
            }
        }

        outTilePropsUsed = finalTilePropsUsed;
        return highestProb;
    }
}
