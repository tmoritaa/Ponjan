using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DeckZone : Zone {
    public DeckZone() : base(Zone.ZoneType.Deck) { }

    public Tile Draw() {
        Tile tile = this.tiles.Last();
        this.tiles.RemoveAt(this.tiles.Count - 1);
        return tile;
    }

    // Fisher-Yates algorithm with adjustments for Random.Range being [min:inclusive, max:exclusive].
    public void Shuffle() {
        for (int i = this.tiles.Count - 1; i > 0; --i) {
            int rand = UnityEngine.Random.Range(0, i + 1);
            Tile tmp = this.tiles[i];
            this.tiles[i] = this.tiles[rand];
            this.tiles[rand] = tmp;
        }
    }

    // Should be called in game start phase.
    public void InitDeckIndicesForPlayers(int diceRoll, List<Player> players) {
        int deckSizePerPlayer = this.tiles.Count / players.Count;
        
        for (int i = 0; i < players.Count; ++i) {
            Player owner = players[i];
            for (int j = 0; j < deckSizePerPlayer; ++j) {
                this.tiles[j + i * deckSizePerPlayer].Owner = owner;
                this.tiles[j + i * deckSizePerPlayer].DeckIdx = j;
            }
        }

        int bossIdx = players.FindIndex(p => p.IsBoss);
        int startTilePlayerIdx = ((diceRoll - 1) % players.Count + bossIdx) % players.Count;
        int startTileIdx = ((startTilePlayerIdx * deckSizePerPlayer + (deckSizePerPlayer - 1) - (diceRoll * 2)) + this.tiles.Count) % this.tiles.Count;

        List<Tile> reorderedTiles = new List<Tile>();
        int counter = 0;
        while (counter < this.tiles.Count) {
            int idx = (startTileIdx + counter + 1) % this.tiles.Count;
            reorderedTiles.Add(this.tiles[idx]);
            ++counter;
        }
        this.tiles = reorderedTiles;
    }
}
