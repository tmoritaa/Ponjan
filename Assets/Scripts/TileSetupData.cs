using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class TileSetupData : ScriptableObject {
    [System.Serializable]
    public class TileSetupEntry {
        public Tile.TileType type;
        public int id;
        public int numberInDeck;
    }

    [SerializeField]
    List<TileSetupEntry> tileSetups;
    public List<TileSetupEntry> TileSetups {
        get { return this.tileSetups; }
    }
}
