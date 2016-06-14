using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DeckGO : MonoBehaviour {
    List<TileGO> deck = new List<TileGO>();

    public void Initialize() {
        int deckSizePerPlayer = CombatSceneController.Instance.MaxDeckSize / CombatSceneController.Instance.PlayerSize;

        TileGO tilePrefab = CombatSceneController.Instance.TilePrefab;
        float imageWidth = tilePrefab.Image.rectTransform.rect.width;
        float imageHeight = tilePrefab.Image.rectTransform.rect.height;
        for (int i = 0; i < deckSizePerPlayer; ++i) {
            bool bot = (i % 2) == 0;
            TileGO tileGO = GameObject.Instantiate(tilePrefab);
            RectTransform rectTrans = tileGO.GetComponent<RectTransform>();
            rectTrans.anchorMin = new Vector2(0, rectTrans.anchorMin.y);
            rectTrans.anchorMax = new Vector2(0, rectTrans.anchorMax.y);
            rectTrans.localPosition = new Vector2(imageWidth / 2 + ((int)(i / 2)) * imageWidth, (bot ? -imageHeight / 2 : imageHeight / 2));
            tileGO.transform.SetParent(this.transform, false);
            this.deck.Add(tileGO);
        }
    }

    public void UpdateZone(Player player, DeckZone deckZone) {
        List<Tile> playerTiles = deckZone.Tiles.FindAll(t => t.Owner == player);

        foreach(TileGO go in this.deck) {
            go.gameObject.SetActive(false);
        }

        foreach(Tile tile in playerTiles) {
            this.deck[tile.DeckIdx].gameObject.SetActive(transform);
            this.deck[tile.DeckIdx].Tile = tile;
            this.deck[tile.DeckIdx].UpdateTile();
        }
    }
}
