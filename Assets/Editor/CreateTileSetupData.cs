using UnityEngine;
using UnityEditor;
using System.Collections;

public class CreateTileSetupData {
    [MenuItem("Assets/Create/TileSetupData")]
    public static void CreateAsset() {
        ScriptableObjectUtility.CreateAsset<TileSetupData>();
    }
}