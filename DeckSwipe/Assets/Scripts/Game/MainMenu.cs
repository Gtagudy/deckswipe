using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    public string sceneName;
    public void NextScene()
    {
        try
        {
            SceneManager.LoadScene(sceneName);
        }
        catch { }
    }
}
