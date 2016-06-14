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

        // If tile is in deck or is controlled by AI, should be hidden.
        if (this.tile.Zone.Type == DeckZone.ZoneType.Deck || this.tile.Owner.ControllerType == Player.PlayerType.AI) {
            this.image.color = new Color(0, 0, 0);
            this.text.text = "";
            return;
        }

        switch(this.tile.Type) {
            case Tile.TileType.Red:
                this.image.color = new Color(1, 0.4f, 0.4f);
                break;
            case Tile.TileType.Blue:
                this.image.color = new Color(0, 0.8f, 0.8f);
                break;
            case Tile.TileType.Yellow:
                this.image.color = new Color(1, 1, 0.6f);
                break;
            case Tile.TileType.White:
                this.image.color = new Color(1, 1, 1);
                break;
            case Tile.TileType.Dragon:
                this.image.color = new Color(0.4f, 0.8f, 0);
                break;
        }

        this.text.text = this.tile.Id.ToString();
    }

    public void ButtonPressed() {
        CombatSceneController.Instance.TilePressed(this.tile);
    }
}
