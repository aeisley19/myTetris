using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    public TextMeshProUGUI scoreText;
    public static int score = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //scoreText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame

    public void AddToScore(int numOfDeletedRows)
    {
        int points = 0;

        switch (numOfDeletedRows)
        {
            case 1:
                points = 40;
                break;
            case 2:
                points = 100;
                break;
            case 3:
                points = 300;
                break;
            case 4:
                points = 1200;
                break;
        }

        score += points * (Game.level + 1);
        print("score " + score);
        scoreText.text = String.Format("{0:000000}", score);
    }
}
