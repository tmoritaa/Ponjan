using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardInfoManager : MonoBehaviour {
    private Dictionary<string, CardInfo> cardInfoDict = new Dictionary<string, CardInfo>();

    private static CardInfoManager instance;
    public static CardInfoManager Instance {
        get { return CardInfoManager.instance; }
    }

    public CardInfo GetCardInfo(string name) {
        return this.cardInfoDict[name];
    }

    private void AddCardsToDict() {
        // For now just fill CardInfoDict manually.
        CardInfo testCard1 = new TestCard1Info();
        CardInfo testSkill = new TestSkillInfo();
        this.cardInfoDict[testCard1.Name] = testCard1;
        this.cardInfoDict[testSkill.Name] = testSkill;
    }

    void Awake() {
        CardInfoManager.instance = this;
        GameObject.DontDestroyOnLoad(this);

        this.AddCardsToDict();
    }
}
