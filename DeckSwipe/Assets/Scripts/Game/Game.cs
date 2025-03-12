using DeckSwipe.CardModel;
using DeckSwipe.CardModel.DrawQueue;
using DeckSwipe.Gamestate;
using DeckSwipe.Gamestate.Persistence;
using DeckSwipe.World;
using Outfrost;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace DeckSwipe {

	public enum Gamemode
	{
		Survival,
		Endless
	}

	public class Game : MonoBehaviour {

		private const int _saveInterval = 8;
		private const int DAYS_TO_WIN = 5;

		public InputDispatcher inputDispatcher;
		public CardBehaviour cardPrefab;
		public Vector3 spawnPosition;
		public Sprite defaultCharacterSprite;
		public bool loadRemoteCollectionFirst;
		public Timer timerPrefab;

		public CardStorage CardStorage {
			get { return cardStorage; }
		}

		public static Gamemode SelectedGameMode { get; private set; } = Gamemode.Survival;

		public Gamemode Currentmode
		{
			get => SelectedGameMode;
			private set => SelectedGameMode = value;
		}

		private CardStorage cardStorage;
		private ProgressStorage progressStorage;
		private float daysPassedPreviously;
		private float daysLastRun;
		private int saveIntervalCounter;
		private CardDrawQueue cardDrawQueue = new CardDrawQueue();

		#region Weather
		public bool WeatherActive = false;
		public bool Snowstorm = false;
        #endregion

        private void Awake() {
			// Listen for Escape key ('Back' on Android) that suspends the game on Android
			// or ends it on any other platform
			#if UNITY_ANDROID
			inputDispatcher.AddKeyUpHandler(KeyCode.Escape,
					keyCode => {
						AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer")
							.GetStatic<AndroidJavaObject>("currentActivity");
						activity.Call<bool>("moveTaskToBack", true);
					});
			#else
			inputDispatcher.AddKeyDownHandler(KeyCode.Escape,
					keyCode => Application.Quit());
			#endif

			cardStorage = new CardStorage(defaultCharacterSprite, loadRemoteCollectionFirst);
			progressStorage = new ProgressStorage(cardStorage);

			GameStartOverlay.FadeOutCallback = StartGameplayLoop;

        }

        public void SetGameMode(int modeIndex)
        {
            Currentmode = (Gamemode)modeIndex;
            Debug.Log($"Game mode set to: {Currentmode}");

        }

        private void Start() {
			CallbackWhenDoneLoading(StartGame);
		}

		private void StartGame() {
			daysPassedPreviously = progressStorage.Progress.daysPassed;
			GameStartOverlay.StartSequence(progressStorage.Progress.daysPassed, daysLastRun);
		}


		public void RestartGame() {
			progressStorage.Save();
			daysLastRun = progressStorage.Progress.daysPassed - daysPassedPreviously;
			cardDrawQueue.Clear();
			StartGame();
		}


        private void StartGameplayLoop() {
            Debug.Log($"Starting gameplay loop with mode: {Currentmode}");
            Stats.ResetStats();
			WeatherActive = false;
			Snowstorm = false;
			progressStorage.Progress.daysPassed = daysPassedPreviously;
			ProgressDisplay.SetDaysSurvived(0);
			DrawNextCard();
		}

		public void DrawNextCard() {
			timerPrefab.timerOn = false;
			if (Stats.Coal == 0) {
				SpawnCard(cardStorage.SpecialCard("gameover_coal"));
			}
			else if (Stats.Food == 0) {
				SpawnCard(cardStorage.SpecialCard("gameover_food"));
			}
			else if (Stats.Health == 0) {
				SpawnCard(cardStorage.SpecialCard("gameover_health"));
			}
			else if (Stats.Hope == 0) {
				SpawnCard(cardStorage.SpecialCard("gameover_hope"));
			}
			else if(Stats.Population == 0)
			{
				SpawnCard(cardStorage.SpecialCard("gameover_population"));
			}
			else {
				IFollowup followup = cardDrawQueue.Next();
				ICard card = followup?.Fetch(cardStorage) ?? cardStorage.Random();
				if (card.CardText.Contains("(Weather)") && WeatherActive)
				{
					print("weather while already active");
					DrawNextCard();
					return;
				}
				else if (card.CardText.Contains("(Weather)")) WeatherActive = true;
                if (card.CardText.Contains("(Timed)"))
				{
					timerPrefab.timerOn = true;
                }
                SpawnCard(card);
			}
			saveIntervalCounter = (saveIntervalCounter - 1) % _saveInterval;
			if (saveIntervalCounter == 0) {
				progressStorage.Save();
			}
		}

		public void CardActionPerformed() {
			progressStorage.Progress.AddDays(Random.Range(0.5f, 1.5f),
					daysPassedPreviously);

			int daysSurvived = (int)(progressStorage.Progress.daysPassed - daysPassedPreviously);

            ProgressDisplay.SetDaysSurvived(daysSurvived);
            Debug.Log($"Game mode set to: {Currentmode}");

            if (Currentmode == Gamemode.Survival && daysSurvived >= DAYS_TO_WIN)
			{
				WinGame();
			} else
			{
                DrawNextCard();
                Debug.Log($"Game mode set to: {Currentmode}");
            }
				
		}

		public void AddFollowupCard(IFollowup followup) {
			cardDrawQueue.Insert(followup);
		}

		private async void CallbackWhenDoneLoading(Callback callback) {
			await progressStorage.ProgressStorageInit;
			callback();
		}

		private void SpawnCard(ICard card) {
			CardBehaviour cardInstance = Instantiate(cardPrefab, spawnPosition,
					Quaternion.Euler(0.0f, -180.0f, 0.0f));
			cardInstance.Card = card;
			cardInstance.snapPosition.y = spawnPosition.y;
			cardInstance.Controller = this;

		}

		private void WinGame()
		{
			Debug.Log("Congratulations! you survived for " + DAYS_TO_WIN + " days! Check out our other game mode: Endless!");
			progressStorage.ResetProgress();

			progressStorage.Save();

			LoadWinScreen();
		}

        private void LoadWinScreen()
        {
            Debug.Log("Loading WinScreen...");
            PlayerPrefs.SetInt("GameWon", 1);
            SceneManager.LoadScene("WinScreen");
        }

    }

}
