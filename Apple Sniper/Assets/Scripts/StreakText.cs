using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StreakText : MonoBehaviour
{
    public static int scoreStreak;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Text streakDisplay = this.GetComponent<Text>();
        streakDisplay.text = "Streak : " + scoreStreak + "x";

        streakDisplay.fontSize =  (int)10f+Mathf.Clamp(scoreStreak * 5, 10, 40);
    }
}
