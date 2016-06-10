using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public abstract class CardInfo {
    public enum CardType {
        Skill,
        Unit,
        Placement,
    }

    CardType cardType;
    public CardType Type {
        get { return this.cardType; }
    }

    protected string name = "";
    public string Name {
        get { return this.name; }
    }

    protected Cost cost = null;
    public Cost Cost {
        get { return this.cost; }
    }

    protected int health = -1;
    public int Health {
        get { return this.health; }
    }

    protected int attackCycle = -1;
    public int AttackCycle {
        get { return this.attackCycle; }
    }

    protected int attackDamage = -1;
    public int AttackDamage {
        get { return this.attackDamage; }
    }

    protected BoardSquareZone.RangeType attackRangeType = BoardSquareZone.RangeType.None;
    public BoardSquareZone.RangeType AttackRangeType {
        get { return this.attackRangeType; }
    }

    // TODO: later implement effects system.

    public CardInfo(CardType type, string name, Cost cost) {
        this.cardType = type;
        this.name = name;
        this.cost = cost;
    }

    public CardInfo SetHealth(int health) {
        this.health = health;
        return this;
    }

    public CardInfo SetAttackParameters(int attackCycle, int attackDamage, BoardSquareZone.RangeType attackRangeType) {
        this.attackCycle = attackCycle;
        this.attackDamage = attackDamage;

        this.attackRangeType = attackRangeType;

        return this;
    }

    public abstract IEnumerator Cast(Game game, Card card);

    public abstract void PerformCastEffect(Game game);
}
