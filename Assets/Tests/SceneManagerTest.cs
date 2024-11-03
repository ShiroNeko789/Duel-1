using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class SceneManagerTest
{
    private sceneManager sceneManagerScript;

    [SetUp]
    public void SetUp()
    {
        GameObject sceneManagerObj = new GameObject();
        sceneManagerScript = sceneManagerObj.AddComponent<sceneManager>();
    }

    [UnityTest]
    public IEnumerator StartGame_LoadsGameScene()
    {
        sceneManagerScript.StartGame();
        yield return null;

        Assert.AreEqual("Game", SceneManager.GetActiveScene().name);
    }

    [UnityTest]
    public IEnumerator Instructions_LoadsInstructionsScene()
    {
        sceneManagerScript.Instructions();
        yield return null;

        Assert.AreEqual("Instructions", SceneManager.GetActiveScene().name);
    }

    [UnityTest]
    public IEnumerator RestartScene_ReloadsCurrentScene()
    {
        SceneManager.LoadScene("Game");
        yield return null;

        sceneManagerScript.RestartScene();
        yield return null;

        Assert.AreEqual("Game", SceneManager.GetActiveScene().name);
    }

    [UnityTest]
    public IEnumerator QuitGame_ExitsApplication()
    {
        // No direct way to test Application.Quit() in UnityEditor. This test can be observed manually.
        sceneManagerScript.QuitGame();
        yield return null;

        Assert.Pass("QuitGame was called, but can't be directly tested in Unity Editor.");
    }
}
