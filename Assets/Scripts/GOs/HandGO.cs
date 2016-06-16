using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandGO : MonoBehaviour {
    List<TileGO> hand = new List<TileGO>();

	public void Initialize() {
        int maxHandSize = CombatSceneController.MaxPlayerHandSize;

        int midIdx = maxHandSize / 2;

        TileGO tilePrefab = CombatSceneController.Instance.TilePrefab;

        float imageWidth = tilePrefab.Image.rectTransform.rect.width;
        for (int i = 0; i < maxHandSize; ++i) {
            TileGO tile = GameObject.Instantiate(tilePrefab);
            tile.transform.SetParent(this.transform, false);

            tile.transform.localPosition = new Vector2((i - midIdx) * imageWidth, 0);
            this.hand.Add(tile);
        }
    }

    public void UpdateZone(List<Tile> tiles) {
        for(int i = 0; i < this.hand.Count; ++i) {
            if (i < tiles.Count) {
                this.hand[i].gameObject.SetActive(true);
                Tile tile = tiles[i];
                this.hand[i].Tile = tile;
                this.hand[i].UpdateTile();
            } else {
                this.hand[i].gameObject.SetActive(false);
            }
        }
    }
}
