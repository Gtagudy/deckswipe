using System.Collections.Generic;
using DeckSwipe.CardModel;
using DeckSwipe.World;
using UnityEngine;

namespace DeckSwipe.Gamestate {
	
	public static class Stats {
		
		private const int _maxStatValue = 32;
		private const int _startingCoal = 16;
		private const int _startingFood = 16;
		private const int _startingHealth = 16;
		private const int _startingHope = 16;
		private const int _startingPopulation = 8;
		
		private static readonly List<StatsDisplay> _changeListeners = new List<StatsDisplay>();
		
		public static int Coal { get; private set; }
		public static int Food { get; private set; }
		public static int Health { get; private set; }
		public static int Hope { get; private set; }
		public static int Population { get; private set; }
		
		public static float CoalPercentage => (float) Coal / _maxStatValue;
		public static float FoodPercentage => (float) Food / _maxStatValue;
		public static float HealthPercentage => (float) Health / _maxStatValue;
		public static float HopePercentage => (float) Hope / _maxStatValue;
		public static float PopPercentage => (float) Population / _maxStatValue;

		private static int StartingValue = _maxStatValue / 2; //16 in base
		private static int BareSurvival = _maxStatValue / 4;

			
		public static void ApplyModification(StatsModification mod) {
			int PopulationDeficit = (int)Mathf.Log(Population / (_maxStatValue / 4) + ((Food + Hope) / StartingValue));

			Coal = ClampValue(Coal + mod.coal - (PopulationDeficit / 2));
			Food = ClampValue(Food + mod.food - (PopulationDeficit / 3 * 4));
			Health = ClampValue(Health + mod.health - (PopulationDeficit * 2/3));
			Hope = ClampValue(Hope + mod.hope + (PopulationDeficit / 3));
			Population = ClampValue(Population + mod.pop + (PopulationDeficit * 3/2));
			TriggerAllListeners();
		}
		
		public static void ResetStats() {
			ApplyStartingValues();
			TriggerAllListeners();
		}
		
		private static void ApplyStartingValues() {
			Coal = ClampValue(_startingCoal);
			Food = ClampValue(_startingFood);
			Health = ClampValue(_startingHealth);
			Hope = ClampValue(_startingHope);
			Population = ClampValue(_startingPopulation);
		}

        public static void ApplyPreview(StatsModification mod)
        {
            TriggerAllPreview(mod);
        }

        private static void TriggerAllPreview(StatsModification mod)
        {
            for (int i = 0; i < _changeListeners.Count; i++)
            {
                if (_changeListeners[i] == null)
                {
                    _changeListeners.RemoveAt(i);
                }
                else
                {
                    _changeListeners[i].TriggerPreview(mod);
                }
            }
        }

        private static void TriggerAllListeners() {
			for (int i = 0; i < _changeListeners.Count; i++) {
				if (_changeListeners[i] == null) {
					_changeListeners.RemoveAt(i);
				}
				else {
					_changeListeners[i].TriggerUpdate();
				}
			}
		}
		
		public static void AddChangeListener(StatsDisplay listener) {
			_changeListeners.Add(listener);
		}
		
		private static int ClampValue(int value) {
			return Mathf.Clamp(value, 0, _maxStatValue);
		}
		
	}
	
}
