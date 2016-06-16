﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DragonCombination : HandCombination {
    private int id;

    public DragonCombination(int id) : base("Dragon", 1) {
        this.id = id;
    }

    public override bool HandHasCombination(List<Tile> tiles) {
        List<Tile> sets = Tile.ReturnGroupedTiles(tiles);

        bool exists = sets.Exists(t => t.Type == Tile.TileType.Dragon && t.Id == this.id);
        
        bool valid = sets.Count == 3 && exists;

        return valid;
    }
}
