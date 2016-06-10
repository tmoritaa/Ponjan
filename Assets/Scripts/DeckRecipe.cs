using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DeckRecipe {
    public class DeckEntry {
        private CardInfo cardInfo;
        public CardInfo CardInfo {
            get { return this.cardInfo; }
        }
        public int number;

        public DeckEntry(CardInfo info, int number) {
            this.cardInfo = info;
            this.number = number;
        }
    }

    private List<DeckEntry> recipe;
    public List<DeckEntry> Recipe {
        get { return this.recipe; }
    }

    public DeckRecipe(List<DeckEntry> recipe) {
        this.recipe = new List<DeckEntry>(recipe);
    }

}