using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    public void NextScene(string sceneName)
    {
        try
        {
            SceneManager.LoadScene(sceneName);
        }
        catch { }
    }
}
