using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RoundResultsScreen : MonoBehaviour {
    [SerializeField]
    Text text;

    public void UpdateResults(Player player, List<HandCombination> handCombs) {
        string str = "Results for " + player.Id;
        handCombs.ForEach(h => str += "\n" + h.Name + " " + h.Score);
        this.text.text = str;
    }
}
