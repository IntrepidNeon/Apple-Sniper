using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public InputField input;
    public Slider slider;
    public RigidbodyController playerRef;

    public static bool GameIsPaused = false;
    // Start is called before the first frame update
    void Awake()
    {
        pauseMenuUI.SetActive(false);

        input.text = PlayerPrefs.GetFloat("Sensitivity").ToString();
        slider.value = PlayerPrefs.GetFloat("Sensitivity");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;

    }
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;

        input.text = playerRef.getSensitivity().ToString();
        slider.value = playerRef.getSensitivity();
    }
    public void menuSetSensitivity()
    {
        try
        {
            playerRef.setSensitivity(float.Parse(input.text));
            slider.value = playerRef.getSensitivity();
        }
        catch (FormatException e)
        {
            input.text = playerRef.getSensitivity().ToString();
        }
        
    }
    public void sliderSetSensitivity()
    {
        try
        {
            playerRef.setSensitivity(slider.value);
            input.text = playerRef.getSensitivity().ToString();
        }
        catch (FormatException e)
        {
            slider.value = playerRef.getSensitivity();
        }
    }
}
