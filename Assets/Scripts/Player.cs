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

    private DiscardZone discardzone = new DiscardZone();
    public DiscardZone DiscardZone {
        get { return this.discardzone; }
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
        this.discardzone.AddTile(tile);
    }
}
