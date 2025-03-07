using System;
﻿using DeckSwipe.Gamestate;

namespace DeckSwipe.CardModel {

	[Serializable]
	public class StatsModification {

		public int coal;
		public int food;
		public int health;
		public int hope;

		public StatsModification(int coal, int food, int health, int hope) {
			this.coal = coal;
			this.food = food;
			this.health = health;
			this.hope = hope;
		}

		public void Perform() {
			// TODO Pass through status effects
			Stats.ApplyModification(this);
		}

        public void PerformSnowstorm()
        {
            // TODO Pass through status effects
            coal -= 2;
            health -= 2;
            Stats.ApplyModification(this);
        }

        public void Preview()
        {
            // TODO Pass through status effects
            Stats.ApplyPreview(this);
        }

        public void PreviewSnowstorm()
        {
            // TODO Pass through status effects
            coal -= 2;
            health -= 2;
            Stats.ApplyPreview(this);
            coal += 2;
            health += 2;
        }

    }

}
