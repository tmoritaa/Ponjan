using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StealZoneGO : MonoBehaviour {
    List<TileGO> tileGOs = new List<TileGO>(); 
	
    public void Initialize() {
        int numMergeable = CombatSceneController.MaxPlayerHandSize / 3;

        TileGO tilePrefab = CombatSceneController.Instance.TilePrefab;

        int midIdx = CombatSceneController.SetSize / 2;
        float imageWidth = tilePrefab.Image.rectTransform.rect.width;
        float imageHeight = tilePrefab.Image.rectTransform.rect.height;
        for (int i = 0; i < numMergeable; ++i) {
            for (int j = 0; j < CombatSceneController.SetSize; ++j) {
                TileGO tile = GameObject.Instantiate(tilePrefab);
                RectTransform rectTrans = tile.GetComponent<RectTransform>();
                rectTrans.anchorMin = new Vector2(rectTrans.anchorMin.x, 0);
                rectTrans.anchorMax = new Vector2(rectTrans.anchorMax.x, 0);
                rectTrans.localPosition = new Vector2((j - midIdx) * imageWidth, i * imageHeight);
                tile.transform.SetParent(this.transform, false);

                this.tileGOs.Add(tile);
            }
        }
    }

    public void UpdateZone(List<Tile> tiles) {
        for (int i = 0; i < this.tileGOs.Count; ++i) {
            if (i < tiles.Count) {
                this.tileGOs[i].gameObject.SetActive(true);
                Tile tile = tiles[i];
                this.tileGOs[i].Tile = tile;
                this.tileGOs[i].UpdateTile();
            } else {
                this.tileGOs[i].gameObject.SetActive(false);
            }
        }
    }
}
