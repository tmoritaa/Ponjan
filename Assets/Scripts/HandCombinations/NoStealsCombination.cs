using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class NoStealsCombination : HandCombination {
    public NoStealsCombination() : base("No Steals", 1) { }

    public override bool HandHasCombination(List<Tile> tiles) {
        List<Tile> handTiles = tiles.Where(t => t.Zone.Type == Zone.ZoneType.Hand).ToList();

        bool valid = (handTiles.Count == CombatSceneController.MaxPlayerHandSize);
        if (valid) {
            List<Tile> sets = Tile.ReturnGroupedTiles(handTiles);
            valid = (sets.Count == CombatSceneController.MaxPlayerHandSize / CombatSceneController.SetSize);
        }

        return valid;
    }
}
