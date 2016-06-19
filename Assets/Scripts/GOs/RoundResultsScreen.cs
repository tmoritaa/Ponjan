using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RoundResultsScreen : MonoBehaviour {
    [SerializeField]
    Text text;

    public void UpdateResultsForHandCombDisplay(Player player, List<HandCombination> handCombs) {
        int score = Game.CalculateScoreFromCombinations(handCombs, player.IsBoss);
        string str = "Results for " + player.Id;
        handCombs.ForEach(h => str += "\n" + h.Name + " " + h.Score);
        str += "\nAdded Score=" + score;
        this.text.text = str;
    }

    public void UpdateResultsForNoDeckScoring(List<Player> players) {
        List<int> scoresPerPlayer = Game.CalculateScoreForNoDeck(players);

        string str = "Results for Tie";
        foreach(Player player in players) {
            str += "\nPlayer " + player.Id + " " + scoresPerPlayer[player.Id];
        }
        this.text.text = str;
    }
}
