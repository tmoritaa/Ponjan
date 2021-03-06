﻿using UnityEngine;
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

    private int score = 250;
    public int Score {
        get { return this.score; }
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

    private bool hasReached = false;
    public bool HasReached {
        get { return this.hasReached; }
        set { this.hasReached = value; }
    }

    private bool ippatsuPotential = false;
    public bool IppatsuPotential {
        get { return this.ippatsuPotential; }
        set { this.ippatsuPotential = value; }
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

    private bool scoredThisRound = false;
    public bool ScoredThisRound {
        get { return this.scoredThisRound; }
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

    public void StealTileForCompletion(Tile tile, Player stolenPlayer) {
        tile.Owner = this;
        tile.StolenPlayer = stolenPlayer;

        this.handZone.AddTile(tile);
    }

    public void StealTileFromDiscard(Tile tile, Player stolenPlayer) {
        tile.Owner = this;
        tile.StolenPlayer = stolenPlayer;

        List<Tile> mergingTiles = this.handZone.Tiles.FindAll(t => t.IsSame(tile));

        for (int i = 0; i < CombatSceneController.SetSize - 1; ++i) {
            this.stealZone.AddTile(mergingTiles[i]);
        }

        this.stealZone.AddTile(tile);
    }

    public void SortHand() {
        this.handZone.SortHand();
    }

    public bool CanStealTileForMerge(Tile stealTile) {
        List<Tile> hand = new List<Tile>(this.handZone.Tiles);

        // This is to prevent a player to merge the last set of his hand.
        if (hand.Count <= CombatSceneController.SetSize - 1) {
            return false;
        }

        if (this.hasReached) {
            List<Tile> sets = Tile.ReturnGroupedTiles(hand);

            foreach (Tile tile in sets) {
                for (int i = 0; i < CombatSceneController.SetSize; ++i) {
                    hand.Remove(hand.Find(t => t.IsSame(tile)));
                }
            }
        }

        List<Tile> mergeable = Tile.ReturnGroupedTiles(hand, CombatSceneController.SetSize - 1);

        bool canSteal = false;
        foreach (Tile tile in mergeable) {
            if (tile.IsSame(stealTile)) {
                canSteal = true;
                break;
            }
        }

        return canSteal;
    }

    public bool OneAwayFromCompletion() {
        List<Tile> allTiles = new List<Tile>(this.handZone.Tiles);
        allTiles.AddRange(this.stealZone.Tiles);

        return Tile.GetNumberOfTilesToCompleteHand(allTiles) == 1;
    }

    public Tile GetOddOneOutTile() {
        Debug.Assert(this.OneAwayFromCompletion(), "Odd one out tile only makes sense when player is one away from completion");

        List<Tile> allTiles = new List<Tile>(this.handZone.Tiles);
        allTiles.AddRange(this.stealZone.Tiles);

        List<Tile> sets = Tile.ReturnGroupedTiles(allTiles);

        foreach (Tile tile in sets) {
            for (int i = 0; i < CombatSceneController.SetSize; ++i) {
                allTiles.Remove(allTiles.Find(t => t.IsSame(tile)));
            }
        }

        List<Tile> pair = Tile.ReturnGroupedTiles(allTiles, CombatSceneController.SetSize - 1);

        foreach (Tile tile in pair) {
            for (int i = 0; i < CombatSceneController.SetSize - 1; ++i) {
                allTiles.Remove(allTiles.Find(t => t.IsSame(tile)));
            }
        }

        Debug.Assert(allTiles.Count == 1, "Should only be one left");

        return allTiles[0];
    }

    public int GetNumTilesLeftForCompletion() {
        List<Tile> allTiles = new List<Tile>(this.handZone.Tiles);
        allTiles.AddRange(this.stealZone.Tiles);

        return Tile.GetNumberOfTilesToCompleteHand(allTiles);
    }

    public void ScorePointsWithHandComb(List<HandCombination> handCombs, List<Player> playersToTake) {
        int score = Game.CalculateScoreFromCombinations(handCombs, this.isBoss);

        playersToTake.ForEach(p => p.ModifyPoints(-score / playersToTake.Count));

        this.ModifyPoints(score);
    }

    public void ModifyPoints(int score) {
        if (score > 0) {
            this.scoredThisRound = true;
        }

        this.score += score;
    }

    public Tile AIPickTileToDiscard(List<HandCombination> handCombs, Game game) {
        Tile discardTile = null;

        if (!this.HasReached) {
            List<Tile> allTiles = new List<Tile>(this.HandZone.Tiles);
            allTiles.AddRange(this.StealZone.Tiles);
            allTiles.Sort(Tile.CompareTiles);

            List<HandCombProbs> handCombProbs = new List<HandCombProbs>();
            foreach (HandCombination handComb in handCombs) {
                List<Tile.TileProp> tileProp;
                float prob = handComb.GetProbabilityOfCompletion(allTiles, game.TilesInGame, game, out tileProp);

                HandCombProbs combProb = new HandCombProbs(handComb, tileProp, prob);
                handCombProbs.Add(combProb);
            }

            handCombProbs.Sort(HandCombProbs.SortFunc);

            while (handCombProbs.Count > 0 && discardTile == null) {
                HashSet<Tile.TileProp> tilePropSet = new HashSet<Tile.TileProp>();
                foreach (HandCombProbs combProb in handCombProbs) {
                    foreach (Tile.TileProp prop in combProb.necessaryTiles) {
                        tilePropSet.Add(prop);
                    }
                }

                foreach (Tile tile in this.handZone.Tiles) {
                    if (!tilePropSet.Contains(new Tile.TileProp(tile))) {
                        discardTile = tile;
                        break;
                    }
                }

                if (discardTile == null) {
                    handCombProbs.RemoveAt(handCombProbs.Count - 1);
                }
            }
        }

        if (discardTile == null) {
            discardTile = this.handZone.Tiles.Last();
        }
        
        return discardTile;
    }

    public void Reset(Game game) {
        List<Tile> handTiles = new List<Tile>(this.handZone.Tiles);
        handTiles.ForEach(t => game.Deck.AddTile(t));

        List<Tile> discardTiles = new List<Tile>(this.discardZone.Tiles);
        discardTiles.ForEach(t => game.Deck.AddTile(t));

        List<Tile> stealTiles = new List<Tile>(this.stealZone.Tiles);
        stealTiles.ForEach(t => game.Deck.AddTile(t));

        this.isActive = false;
        this.hasReached = false;
        this.ippatsuPotential = false;
        this.scoredThisRound = false;
    }
}
