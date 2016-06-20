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

    [SerializeField]
    Text roundDisplayText;

    public void UpdateText(List<Player> players, int curRound) {
        this.botText.text = players[0].Score.ToString();
        this.rightText.text = players[1].Score.ToString();
        this.leftText.text = players[2].Score.ToString();

        this.botText.color = players[0].IsBoss ? new Color(1, 0, 0) : new Color(0, 0, 0);
        this.rightText.color = players[1].IsBoss ? new Color(1, 0, 0) : new Color(0, 0, 0);
        this.leftText.color = players[2].IsBoss ? new Color(1, 0, 0) : new Color(0, 0, 0);

        this.roundDisplayText.text = "Round " + curRound;
    }
}
