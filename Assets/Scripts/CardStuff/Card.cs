using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Card {
    private CardInfo cardInfo;
    public CardInfo Info {
        get { return this.cardInfo; }
    }

    private Zone zone = null;
    public Zone Zone {
        set { this.zone = value; }
        get { return this.zone; }
    }

    private Player owner;
    public Player Owner {
        get { return this.owner; }
    }

    private int curHealth;
    public int CurHealth {
        get { return this.curHealth; }
    }

    private int curAttackCycle;
    public int CurAttackCycle {
        get { return this.curAttackCycle; }
    }

    public Card(CardInfo info, Player owner) {
        this.owner = owner;
        this.cardInfo = info;
        this.curHealth = this.cardInfo.Health;
        this.curAttackCycle = 0;
    }

    public void IncreaseCurAttackCycle(int incVal) {
        this.curAttackCycle += incVal;
    }

    public bool ReadyToAttack() {
        return this.curAttackCycle >= this.Info.AttackCycle;
    }

    public void ResetAttackCycle() {
        this.curAttackCycle = 0;
    }

    public void DealDamage(int dmg) {
        this.curHealth -= dmg;
    }

    public bool CanPayCost() {
        return this.cardInfo.Cost.IsPayable(this);
    }

    public IEnumerable PayCost(Game game) {
        IEnumerator costEnum = this.cardInfo.Cost.PayCost(game, this);
        bool hasNext = true;
        do {
            hasNext = costEnum.MoveNext();
            yield return 0;
        } while (hasNext);

        yield break;
    }

    public bool ShouldBeDestroyed() {
        return this.CurHealth <= 0;
    }
}
