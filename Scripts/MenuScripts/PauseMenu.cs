using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseMenuUI;

    SaveNLoad playerPosData;

    void Start()
    {
        playerPosData = FindObjectOfType<SaveNLoad>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void LoadMenu()
    {
        SoundManager.Instance.startingZoneBGMusic.Stop();
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void SaveGame()
    {
        Debug.Log("Save Player Position");
        playerPosData.Save();
    }

    public void QuitGame()
    {
        Debug.Log("Quiting game");
        Application.Quit();
    }
}
