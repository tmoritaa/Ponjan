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

    private int curMaterial = 0;
    public int CurMaterial {
        get { return this.curMaterial; }
    }

    private int curHealth = 10;
    public int CurHealth {
        get { return this.curHealth; }
    }

    private bool hasInitiative = false;
    public bool HasInitiative {
        get { return this.hasInitiative; }
        set { this.hasInitiative = value; }
    }

    private BoardSquareZone curLocation;
    public BoardSquareZone CurLocation {
        get { return this.curLocation; }
    }

    private DeckZone deck = null;
    private HandZone hand = new HandZone();
    public HandZone Hand {
        get { return this.hand; }
    }

    private DiscardZone discard = new DiscardZone();
    public DiscardZone Discard {
        get { return this.discard; }
    }

    public Player(PlayerType pType, int id, string name, DeckRecipe recipe) {
        this.controllerType = pType;
        this.id = id;
        this.name = name;
        this.deck = new DeckZone(this, recipe);
    }

    public void Draw(int number = 1) {
        for (int i = 0; i < number; ++i) {
            this.hand.AddCard(this.deck.Draw());
        }
    }

    public void PlayUnitCardFromHand(Card card, BoardSquareZone boardSquare) { 
        boardSquare.AddCard(card);
    }

    public void PlaySkillCardFromHand(Card card) {
        discard.AddCard(card);
    }

    public void DestroyCard(Card card) {
        this.discard.AddCard(card);
    }

    public void AddMaterial(int val) {
        this.curMaterial += val;
    }

    public void MoveToSquare(BoardSquareZone zone) {
        if (this.curLocation != null) {
            this.curLocation.Players[this.id] = null;
        }

        this.curLocation = zone;
        this.curLocation.Players[this.id] = this;
    }

    public void PayCost(int val) {
        this.curMaterial -= val;
    }

    public void TakeDamage(int val) {
        this.curHealth -= val;
    }
}
