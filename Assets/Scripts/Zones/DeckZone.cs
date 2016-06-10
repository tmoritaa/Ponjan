using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DeckZone : Zone {
    public Card Draw() {
        Card card = this.cards.Last();
        this.cards.RemoveAt(this.cards.Count - 1);
        return card;
    }

    // Fisher-Yates algorithm with adjustments for Random.Range being [min:inclusive, max:exclusive].
    public void Shuffle() {
        for (int i = this.cards.Count - 1; i > 0; --i) {
            int rand = UnityEngine.Random.Range(0, i + 1);
            Card tmp = this.cards[i];
            this.cards[i] = this.cards[rand];
            this.cards[rand] = tmp;
        }
    }

    public DeckZone(Player owner, DeckRecipe recipe) : base(Zone.ZoneType.Deck) {
        foreach (DeckRecipe.DeckEntry entry in recipe.Recipe) {
            for (int i = 0; i < entry.number; ++i) {
                Card card = new Card(entry.CardInfo, owner);
                this.AddCard(card);
            }
        }

        this.Shuffle();
    }
}
