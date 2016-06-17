using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ScoreDisplayGO : MonoBehaviour {
    [SerializeField]
    Text rightText;

    [SerializeField]
    Text leftText;

    [SerializeField]
    Text botText;
    
    public void UpdateText(List<Player> players) {
        this.botText.text = players[0].Score.ToString();
        this.rightText.text = players[1].Score.ToString();
        this.leftText.text = players[2].Score.ToString();
    }
}
