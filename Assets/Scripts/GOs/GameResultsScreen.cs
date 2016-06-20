using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameResultsScreen : MonoBehaviour {
    [SerializeField]
    Text text;

    public void DisplayFinalResults(List<Player> players) {
        string str = "Final Results";
        foreach (Player player in players) {
            str += "\nPlayer " + player.Id + " " + player.Score;
        }

        text.text = str;
    }
}
