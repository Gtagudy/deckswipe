using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreenController : MonoBehaviour
{
    [SerializeField] private Button returnToMainMenuButton;

    void Start()
    {
        returnToMainMenuButton.onClick.AddListener(ReturnToMainMenu);
    }

    private void ReturnToMainMenu()
    {
        Debug.Log("Loading MainMenuScene...");
        SceneManager.LoadScene("MainMenuScene");
    }
    
}
