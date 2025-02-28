using DeckSwipe.CardModel.DrawQueue;

namespace DeckSwipe.CardModel {

	public class ActionOutcome : IActionOutcome {

		private readonly StatsModification statsModification;
		private readonly IFollowup followup;

		public ActionOutcome() {
			statsModification = new StatsModification(0, 0, 0, 0, 0);
		}

		public ActionOutcome(int coalMod, int foodMod, int healthMod, int hopeMod, int populationMod) {
			statsModification = new StatsModification(coalMod, foodMod, healthMod, hopeMod, populationMod);
		}

		public ActionOutcome(int coalMod, int foodMod, int healthMod, int hopeMod, int populationMod, IFollowup followup) {
			statsModification = new StatsModification(coalMod, foodMod, healthMod, hopeMod, populationMod);
			this.followup = followup;
		}

		public ActionOutcome(StatsModification statsModification, IFollowup followup) {
			this.statsModification = statsModification;
			this.followup = followup;
		}

		public void Perform(Game controller) {
			statsModification.Perform();
			if (followup != null) {
				controller.AddFollowupCard(followup);
			}
			controller.CardActionPerformed();
		}

	}

}
