using UnityEngine;
using System.Collections;

public class PlayerZoneGO : MonoBehaviour {
    [SerializeField]
    private PlayerInfoGO playerInfoGO = null;

    [SerializeField]
    private HandGO handGO = null;
    
    public void Initialize(Player player) {
        this.playerInfoGO.Player = player;
        this.handGO.Player = player;
    }
}
