using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TileGO : MonoBehaviour {
    [SerializeField]
    Image image;
    public Image Image {
        get { return this.image; }
    }

    [SerializeField]
    Text text;

    private Tile tile;
    public Tile Tile {
        set { this.tile = value; }
    }

    public void UpdateTile() {
        if (this.tile == null) {
            return;
        }

        switch(this.tile.Type) {
            case Tile.TileType.Red:
                this.image.color = new Color(1, 0, 0);
                break;
            case Tile.TileType.Blue:
                this.image.color = new Color(0, 0, 1);
                break;
            case Tile.TileType.Yellow:
                this.image.color = new Color(1, 1, 0);
                break;
            case Tile.TileType.White:
                this.image.color = new Color(1, 1, 1);
                break;
            case Tile.TileType.Dragon:
                this.image.color = new Color(0, 1, 0);
                break;
        }

        this.text.text = this.tile.Id.ToString();
    }

    public void ButtonPressed() {
        CombatSceneController.Instance.TilePressed(this.tile);
    }
}
