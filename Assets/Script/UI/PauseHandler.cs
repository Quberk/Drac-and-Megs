using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseHandler : MonoBehaviour
{
    private GameObject pauseBtn;

    private GameObject pauseMenu;

    [Header("Resume")]
    public int waitTime;
    private int waitCounter;
    private float coolDownCounter = 0;
    private Text resWaitTime;
    private GameObject resumeWaiting;
    private bool counting = false;
    private float deltaTime;

    [HideInInspector]
    public float normalTimeSCale;

    // Start is called before the first frame update
    void Start()
    {
        normalTimeSCale = Time.timeScale;
        pauseBtn = GameObject.Find("Pause_Button");
        pauseMenu = GameObject.Find("Pause_Menu");
        resumeWaiting = GameObject.Find("Resume_Waiting_Time");
        resWaitTime = GameObject.Find("Resume_Time").GetComponent<Text>();

        resumeWaiting.SetActive(false);
        pauseMenu.SetActive(false);

        waitCounter = waitTime;
        deltaTime = Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (counting == true)
        {
            
            resWaitTime.text = waitCounter.ToString();
            coolDownCounter += deltaTime;
            if (coolDownCounter >= 1f)
            {
                waitCounter--;
                coolDownCounter = 0f;

                if (waitCounter == 0)
                {
                    counting = false;
                    resumeWaiting.SetActive(false);
                    waitCounter = waitTime;
                    Time.timeScale = normalTimeSCale;
                } 
            }
        }
    }

    public void PauseButton()
    {
        pauseBtn.SetActive(false);
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeButton()
    {
        pauseBtn.SetActive(true);
        pauseMenu.SetActive(false);
        resumeWaiting.SetActive(true);
        counting = true;
        
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
        Time.timeScale = normalTimeSCale;
    }

    public void HighScoreButton()
    {
        SceneManager.LoadScene("HighScoreScene", LoadSceneMode.Single);
        Time.timeScale = normalTimeSCale;
    }

    public void ExitButton()
    {
        Application.Quit();
        Time.timeScale = normalTimeSCale;
    }
}
