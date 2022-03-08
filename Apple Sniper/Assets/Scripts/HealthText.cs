using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthText : MonoBehaviour
{
    public static string healthString;
    public static Vector3 healthColor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Text healthDisplay = this.GetComponent<Text>();
        healthDisplay.color = new Color(healthColor.x, healthColor.y, healthColor.z);
        healthDisplay.text = healthString;
    }
}
