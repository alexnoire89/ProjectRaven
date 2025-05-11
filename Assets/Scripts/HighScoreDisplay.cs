using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;


    private void Start()
    {

        if (scoreText != null)
        {
            int score = SaveDataSystem.Instance.GetSessionBestScore();
            scoreText.text = score.ToString();


        }

        if (highScoreText != null)
        {
            int finalScore = SaveDataSystem.Instance.GetHighScore();
            highScoreText.text = finalScore.ToString();
        }
    }
}

