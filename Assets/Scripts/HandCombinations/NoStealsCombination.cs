using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class NoStealsCombination : HandCombination {
    public NoStealsCombination() : base("No Steals", 1) { }

    public override bool HandHasCombination(List<Tile> tiles, HandCombination.CompletionType compType) {
        // We count discard as well for winning off of stealing, which shouldn't count as a merge.
        List<Tile> handTiles = tiles.Where(t => t.Zone.Type == Zone.ZoneType.Hand || t.Zone.Type == Zone.ZoneType.Discard).ToList();

        bool valid = (handTiles.Count == CombatSceneController.MaxPlayerHandSize);
        if (valid) {
            List<Tile> sets = Tile.ReturnGroupedTiles(handTiles);
            valid = (sets.Count == CombatSceneController.MaxPlayerHandSize / CombatSceneController.SetSize);
        }

        return valid;
    }

    public override float GetProbabilityOfCompletion(List<Tile> _tiles, List<Tile.TileProp> allTileData, Game game, out List<Tile.TileProp> outTilePropsUsed) {
        outTilePropsUsed = new List<Tile.TileProp>();
        return 0;
    }
}
