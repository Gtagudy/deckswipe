using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DeckSwipe.Gamestate;
using DeckSwipe.Gamestate.Persistence;


public class WinScreenController : MonoBehaviour
{
    [SerializeField] private Button returnToMainMenuButton;

    private ProgressStorage progressStorage; 

    void Start()
    {
        returnToMainMenuButton.onClick.AddListener(ReturnToMainMenu);

        if (PlayerPrefs.GetInt("GameWon", 0) == 1)
        {
            ResetProgress();
            PlayerPrefs.DeleteKey("GameWon");
        }
    }

    private void ResetProgress()
    {
        progressStorage = new ProgressStorage(new CardStorage(null, false));

        StartCoroutine(WaitForInitAndReset());
    }

    private System.Collections.IEnumerator WaitForInitAndReset()
    {
        yield return new WaitUntil(() => progressStorage.ProgressStorageInit.IsCompleted);

        progressStorage.ResetProgress();
        progressStorage.Save();
        Debug.Log("Game progress has been reset.");
    }
    private void ReturnToMainMenu()
    {
        Debug.Log("Loading MainMenuScene...");
        SceneManager.LoadScene("MainMenuScene");
    }
    
}
