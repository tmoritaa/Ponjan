using UnityEngine;
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
    }

    public void UpdateZones() {
        this.handGO.UpdateZone(this.player.HandZone.Tiles);
    }
}
