﻿using UnityEngine;
using System.Collections;

public class PlayerZoneGO : MonoBehaviour {
    [SerializeField]
    private Player player;

    [SerializeField]
    private HandGO handGO;

    [SerializeField]
    private DiscardGO discardGO;

    [SerializeField]
    private DeckGO deckGO;
    
    public void Initialize(Player player) {
        this.player = player;

        this.handGO.Initialize();
        this.discardGO.Initialize();
        this.deckGO.Initialize();
    }

    public void UpdateZones(Game game) {
        this.handGO.UpdateZone(this.player.HandZone.Tiles);
        this.discardGO.UpdateZone(this.player.DiscardZone.Tiles);
        this.deckGO.UpdateZone(this.player, game.Deck);
    }
}
