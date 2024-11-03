using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;  // Reference to the Pause Menu UI

    private bool isPaused = false;

    void Update()
    {
        // Toggle pause when Escape key is pressed
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
        pauseMenuUI.SetActive(false);         // Hide the Pause Menu UI
        Time.timeScale = 1f;                  // Resume game time
        isPaused = false;
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);          // Show the Pause Menu UI
        Time.timeScale = 0f;                  // Freeze game time
        isPaused = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;                  // Ensure game time is normal on restart
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }

    public void QuitGame()
    {
        Application.Quit();                   // Quit the application
        Debug.Log("Game Quit");               // Log quit event (helpful for testing in the editor)
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

