using DeckSwipe.CardModel;
using DeckSwipe.Gamestate;
using Outfrost;
using UnityEngine;
using UnityEngine.UI;

namespace DeckSwipe.World {
	
	public class StatsDisplay : MonoBehaviour {
		
		public Image coalBar;
		public Image foodBar;
		public Image healthBar;
		public Image hopeBar;
		public Image populationBar;
		public float relativeMargin;
		
		private float minFillAmount;
		private float maxFillAmount;
		
		private void Awake() {
			minFillAmount = Mathf.Clamp01(relativeMargin);
			maxFillAmount = Mathf.Clamp01(1.0f - relativeMargin);
			
			if (!Util.IsPrefab(gameObject)) {
				Stats.AddChangeListener(this);
				TriggerUpdate();
			}
		}

		private Color Good = new Color32(100, 230, 100, 255);
		private Color Bad = new Color32(230, 100, 100, 255);
		private Color Neutral = new Color32(230, 230, 230, 255);
		public void TriggerPreview(StatsModification mod)
		{
			if (mod.coal > 0) coalBar.color = Good;
			else if (mod.coal < 0) coalBar.color = Bad;
			else coalBar.color = Neutral;

            if (mod.food > 0) foodBar.color = Good;
            else if (mod.food < 0) foodBar.color = Bad;
            else foodBar.color = Neutral;

            if (mod.health > 0) healthBar.color = Good;
            else if (mod.health < 0) healthBar.color = Bad;
            else healthBar.color = Neutral;

            if (mod.hope > 0) hopeBar.color = Good;
            else if (mod.hope < 0) hopeBar.color = Bad;
            else hopeBar.color = Neutral;

			if (mod.pop > 0) populationBar.color = Good;
			else if(mod.pop < 0) populationBar.color = Bad;
			else populationBar.color = Neutral;
        }
		
		public void TriggerUpdate() {
			coalBar.fillAmount = Mathf.Lerp(minFillAmount, maxFillAmount, Stats.CoalPercentage);
			coalBar.color = Neutral;
			foodBar.fillAmount = Mathf.Lerp(minFillAmount, maxFillAmount, Stats.FoodPercentage);
			foodBar.color = Neutral;
			healthBar.fillAmount = Mathf.Lerp(minFillAmount, maxFillAmount, Stats.HealthPercentage);
			healthBar.color = Neutral;
			hopeBar.fillAmount = Mathf.Lerp(minFillAmount, maxFillAmount, Stats.HopePercentage);
			hopeBar.color = Neutral;
			populationBar.fillAmount = Mathf.Lerp(minFillAmount, maxFillAmount, Stats.PopPercentage);
			populationBar.color = Neutral;
		}
		
	}
	
}
