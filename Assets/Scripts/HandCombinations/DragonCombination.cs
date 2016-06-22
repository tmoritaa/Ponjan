using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DragonCombination : HandCombination {
    private int id;

    public DragonCombination(int id) : base("Dragon" + id, 1) {
        this.id = id;
    }

    public override bool HandHasCombination(List<Tile> tiles, HandCombination.CompletionType compType) {
        List<Tile> sets = Tile.ReturnGroupedTiles(tiles);

        bool exists = sets.Exists(t => t.Type == Tile.TileType.Dragon && t.Id == this.id);
        
        bool valid = sets.Count == 3 && exists;

        return valid;
    }

    public override float GetProbabilityOfCompletion(List<Tile> _tiles, List<Tile.TileProp> allTileData, Game game, out List<Tile.TileProp> outTilePropsUsed) {
        throw new NotImplementedException();
    }
}
