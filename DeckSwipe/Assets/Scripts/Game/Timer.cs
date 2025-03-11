using DeckSwipe;
using DeckSwipe.CardModel;
using DeckSwipe.Gamestate;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float time;
    public float defaultTime = 10;
    public bool timerOn = false;
    public Game game;

    void Update()
    {
        if (timerOn)
        {
            timerText.enabled = true;
            time -= Time.deltaTime;
            timerText.text = "Think fast!: " + ((int)time).ToString();
            if (time <= 0.0f)
            {
                TimerEnd();
            }
        }
        else
        {
            timerText.enabled = false;
            time = defaultTime;
        }
    }

    public void TimerEnd()
    {
        print("timer end");
        timerOn = false;
        game.CardActionPerformed();
    }
}
