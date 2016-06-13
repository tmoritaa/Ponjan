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
            tileGO.transform.SetParent(this.transform, false);
            RectTransform rectTrans = tileGO.GetComponent<RectTransform>();
            rectTrans.anchorMin = new Vector2(0, rectTrans.anchorMin.y);
            rectTrans.anchorMax = new Vector2(0, rectTrans.anchorMax.y);
            rectTrans.localPosition = new Vector2(imageWidth / 2 + i * imageWidth, 0);
            discard.Add(tileGO);
        }
    }

    public void UpdateZone(List<Tile> tiles) {
        for(int i = 0; i < discard.Count; ++i) {
            if (i < tiles.Count) {
                discard[i].gameObject.SetActive(true);
                Tile tile = tiles[i];
                discard[i].Tile = tile;
                discard[i].UpdateTile();
            } else {
                discard[i].gameObject.SetActive(false);
            }
        }
    }
}
