﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TileGO : MonoBehaviour {
    [SerializeField]
    private Image image;
    public Image Image {
        get { return this.image; }
    }

    [SerializeField]
    private Text text;

    private Vector3 initRotation;

    private Tile tile;
    public Tile Tile {
        set { this.tile = value; }
    }

    public void Awake() {
        this.initRotation = this.transform.localEulerAngles;
    }

    public void UpdateTile() {
        if (this.tile == null) {
            return;
        }

        if (!this.tile.UsedForReach) {
            this.transform.localEulerAngles = this.initRotation;
        } else {
            this.transform.localEulerAngles = this.initRotation + new Vector3(0, 0, 90);
        }

        // If tile is in deck or is controlled by AI, should be hidden.
        if ((!CombatSceneController.Instance.DebugShowDeck && this.tile.Zone.Type == DeckZone.ZoneType.Deck) || 
            (!CombatSceneController.Instance.DebugShowHands && 
                this.tile.Zone.Type == Zone.ZoneType.Hand && 
                this.tile.Owner.ControllerType == Player.PlayerType.AI)) {
            this.image.color = new Color(0.4f, 0.4f, 0.4f);
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
