using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public abstract class Zone {
    public enum ZoneType {
        Hand,
        Deck,
        Gameboard,
        Discard,
    }

    protected ZoneType type;
    public ZoneType Type {
        get { return this.type; }
    }

    protected List<Card> cards = new List<Card>();
    public List<Card> Cards {
        get { return this.cards; }
    }

    public Zone(ZoneType type) {
        this.type = type;
    }

    public virtual void AddCard(Card card) {
        if (card.Zone != null) {
            card.Zone.RemoveCard(card);
        }
        
        card.Zone = this;
        this.cards.Add(card);
    }

    public virtual void RemoveCard(Card card) {
        this.cards.Remove(card);
        card.Zone = null;
    }
}
