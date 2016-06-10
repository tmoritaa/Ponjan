using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SquareCardGO : MonoBehaviour {
    [SerializeField]
    private Text nameText;

    [SerializeField]
    private Text healthText;

    [SerializeField]
    private Text attackCycleText;

    [SerializeField]
    private Text damageText;
	
    public void UpdateSquareCard(Card card) {
        this.nameText.text = card.Info.Name;
        this.healthText.text = "H: " + card.CurHealth + "/" + card.Info.Health;
        this.attackCycleText.text = "A: " + card.CurAttackCycle + "/" + card.Info.AttackCycle;
        this.damageText.text = "D: " + card.Info.AttackDamage;
    }
}
