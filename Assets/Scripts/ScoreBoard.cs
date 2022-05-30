using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] Text currentScore;
    [SerializeField] Text highestScore;
    [SerializeField] int pointsMultiplier;
    // Start is called before the first frame update
    float gameStartedAt;
    bool gameRunning = false;

    void Start()
    {
        if(highestScore != null)
            highestScore.text = PlayerPrefs.GetInt("HighestScore", 0).ToString();

        PlayerController.PlayerStarted += OnPlayerStarted;
        DieColliderController.PlayerDied += OnPlayerDied;
    }

    private void OnPlayerDied()
    {
        gameRunning = false;
        CheckScore();
    }

    private void CheckScore()
    {
        if (Int32.Parse(currentScore.text) > Int32.Parse(highestScore.text))
            PlayerPrefs.SetInt("HighestScore", Int32.Parse(currentScore.text));
    }

    private void OnPlayerStarted()
    {
        gameRunning = true;
        gameStartedAt = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameRunning)
        {
            currentScore.text = (Mathf.CeilToInt(Time.time - gameStartedAt) * pointsMultiplier).ToString();
        }
    }
}
