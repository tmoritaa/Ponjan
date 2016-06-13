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

            Vector3 pos = new Vector2((i - midIdx) * imageWidth, 0);
            tile.transform.localPosition = pos;
            this.hand.Add(tile);
        }
    }

    public void UpdateZone(List<Tile> tiles) {
        for(int i = 0; i < hand.Count; ++i) {
            if (i < tiles.Count) {
                hand[i].gameObject.SetActive(true);
                Tile tile = tiles[i];
                hand[i].Tile = tile;
                hand[i].UpdateTile();
            } else {
                hand[i].gameObject.SetActive(false);
            }
        }
    }
}
