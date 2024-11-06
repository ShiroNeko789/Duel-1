using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void Instructions()
    {
        SceneManager.LoadScene("Instructions");
    }

    public void RestartScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void Levels()
    {
        SceneManager.LoadScene("Levels");
    }

    public void InstructionsM()
    {
        SceneManager.LoadScene("InstructionsM");
    }

    public void Mutliplayer()
    {
        SceneManager.LoadScene("Multiplayer");
    }
}
