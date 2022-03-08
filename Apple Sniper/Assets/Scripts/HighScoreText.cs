using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreText : MonoBehaviour
{
    public static int highScore = 0;
    // Start is called before the first frame update
    void Awake()
    {
        if (PlayerPrefs.HasKey("HighScore"))
        {
             highScore = PlayerPrefs.GetInt("HighScore");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Text highScoreDisplay = this.GetComponent<Text>();
        highScoreDisplay.text = "High Score : " + highScore.ToString();
        if (ScoreText.score > highScore)
        {
            highScore = ScoreText.score;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
    }
}
