using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DiscardGO : MonoBehaviour {
    List<TileGO> discard = new List<TileGO>();

    public void Initialize() {
        CombatSceneController controller = CombatSceneController.Instance;
        int maxDiscardSize = (controller.MaxDeckSize - controller.PlayerSize * (CombatSceneController.MaxPlayerHandSize - 1)) / controller.PlayerSize;

        TileGO tilePrefab = controller.TilePrefab;
        float imageWidth = tilePrefab.Image.rectTransform.rect.width;
        for(int i = 0; i < maxDiscardSize; ++i) {
            TileGO tileGO = GameObject.Instantiate(tilePrefab);
            RectTransform rectTrans = tileGO.GetComponent<RectTransform>();
            rectTrans.anchorMin = new Vector2(0, rectTrans.anchorMin.y);
            rectTrans.anchorMax = new Vector2(0, rectTrans.anchorMax.y);
            rectTrans.localPosition = new Vector2(imageWidth / 2 + i * imageWidth, 0);
            tileGO.transform.SetParent(this.transform, false);
            this.discard.Add(tileGO);
        }
    }

    public void UpdateZone(List<Tile> tiles) {
        for(int i = 0; i < this.discard.Count; ++i) {
            if (i < tiles.Count) {
                this.discard[i].gameObject.SetActive(true);
                Tile tile = tiles[i];
                this.discard[i].Tile = tile;
                this.discard[i].UpdateTile();
            } else {
                this.discard[i].gameObject.SetActive(false);
            }
        }
    }
}
