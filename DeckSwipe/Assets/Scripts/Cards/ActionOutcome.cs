using DeckSwipe.CardModel.DrawQueue;

namespace DeckSwipe.CardModel {

	public class ActionOutcome : IActionOutcome {

		private readonly StatsModification statsModification;
		private readonly IFollowup followup;

		public ActionOutcome() {
			statsModification = new StatsModification(0, 0, 0, 0, 0);
		}

		public ActionOutcome(int coalMod, int foodMod, int healthMod, int hopeMod, int popMod) {
			statsModification = new StatsModification(coalMod, foodMod, healthMod, hopeMod, popMod);
		}

		public ActionOutcome(int coalMod, int foodMod, int healthMod, int hopeMod, int popMod, IFollowup followup) {
			statsModification = new StatsModification(coalMod, foodMod, healthMod, hopeMod, popMod);
			this.followup = followup;
		}

		public ActionOutcome(StatsModification statsModification, IFollowup followup) {
			this.statsModification = statsModification;
			this.followup = followup;
		}

		public void Perform(Game controller) {
			if (controller.Snowstorm)
			{
                statsModification.PerformSnowstorm();
            }
			else statsModification.Perform();
			if (followup != null) {
				if (followup is SpecialFollowup)
				{
					SpecialFollowup weatherCheck = (SpecialFollowup) followup;
					if (weatherCheck.id == "snowStart")
					{
						controller.Snowstorm = true;
						//controller.WeatherActive = true;
                        controller.AddFollowupCard(new Followup(12, 5));
                    }
					else if (weatherCheck.id == "snowEnd")
					{
						controller.Snowstorm = false;
						controller.WeatherActive = false;
					}
				}
				else controller.AddFollowupCard(followup);
			}
			controller.CardActionPerformed();
		}

        public void Preview(Game controller)
        {
            if (controller.Snowstorm)
            {
                statsModification.PreviewSnowstorm();
            }
            statsModification.Preview();
        }

    }

}
