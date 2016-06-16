using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class Player {
    public enum PlayerType {
        Human,
        AI,
    }
    private PlayerType controllerType;
    public PlayerType ControllerType {
        get { return this.controllerType; }
    }

    private int id = -1;
    public int Id {
        get { return this.id; }
    }

    private string name = "";
    public string Name {
        get { return this.name; }
    }

    private bool isBoss = false;
    public bool IsBoss {
        get { return this.isBoss; }
        set { this.isBoss = value; }
    }

    private bool isActive = false;
    public bool IsActive {
        get { return this.isActive; }
        set { this.isActive = value; }
    }

    private HandZone handZone = new HandZone();
    public HandZone HandZone {
        get { return this.handZone; }
    }

    private DiscardZone discardZone = new DiscardZone();
    public DiscardZone DiscardZone {
        get { return this.discardZone; }
    }

    private StealZone stealZone = new StealZone();
    public StealZone StealZone {
        get { return this.stealZone; }
    }

    public Player(PlayerType pType, int id, string name) {
        this.controllerType = pType;
        this.id = id;
        this.name = name;
    }

    public void Draw(int number, DeckZone deck) {
        for (int i = 0; i < number; ++i) {
            Tile tile = deck.Draw();
            tile.Owner = this;
            this.handZone.AddTile(tile);
        }
    }

    public void DiscardFromHand(Tile tile) {
        this.discardZone.AddTile(tile);
    }

    public void StealTileFromDiscard(Tile tile) {
        tile.Owner = this;

        List<Tile> mergingTiles = this.handZone.Tiles.FindAll(t => t.IsSame(tile));

        for (int i = 0; i < CombatSceneController.SetSize - 1; ++i) {
            this.stealZone.AddTile(mergingTiles[i]);
        }

        this.stealZone.AddTile(tile);
    }

    public void SortHand() {
        this.handZone.SortHand();
    }

    public bool CanStealTile(Tile tile) {
        List<Tile> mergeable = Tile.ReturnGroupedTiles(this.handZone.Tiles, CombatSceneController.SetSize - 1);

        bool canSteal = false;
        foreach (Tile otherTile in mergeable) {
            if (otherTile.IsSame(tile)) {
                canSteal = true;
                break;
            }
        }

        return canSteal;
    }

    public void Reset(Game game) {
        List<Tile> handTiles = new List<Tile>(this.handZone.Tiles);
        handTiles.ForEach(t => game.Deck.AddTile(t));

        List<Tile> discardTiles = new List<Tile>(this.discardZone.Tiles);
        discardTiles.ForEach(t => game.Deck.AddTile(t));

        List<Tile> stealTiles = new List<Tile>(this.stealZone.Tiles);
        stealTiles.ForEach(t => game.Deck.AddTile(t));

        this.isActive = false;
        this.isBoss = false;
    }
}
