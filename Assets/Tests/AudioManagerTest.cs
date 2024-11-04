using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class AudioManagerTest
{
    private GameObject gameObj;
    private AudioManager audioManager;

    private AudioClip testClip0;
    private AudioClip testClip1;
    private AudioClip testClip2;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        gameObj = new GameObject();
        audioManager = gameObj.AddComponent<AudioManager>();

        // Ensure AudioSources are created in AudioManager
        audioManager.bgmSource = gameObj.AddComponent<AudioSource>();
        audioManager.sfxSource = gameObj.AddComponent<AudioSource>();

        // Ensure an AudioListener is present for audio tests
        gameObj.AddComponent<AudioListener>();

        // Load test audio clips from Resources folder
        testClip0 = Resources.Load<AudioClip>("TestClip0");
        testClip1 = Resources.Load<AudioClip>("TestClip1");
        testClip2 = Resources.Load<AudioClip>("TestClip2");

        // Check if clips are loaded
        Assert.NotNull(testClip0, "Test audio clip 'TestClip0' is missing in Resources folder.");
        Assert.NotNull(testClip1, "Test audio clip 'TestClip1' is missing in Resources folder.");
        Assert.NotNull(testClip2, "Test audio clip 'TestClip2' is missing in Resources folder.");

        // Assign loaded clips to the sfxClips array
        audioManager.sfxClips = new AudioClip[] { testClip0, testClip1, testClip2 };

        yield return null;
    }

    [UnityTest]
    public IEnumerator PlaySFX_ValidIndex_ShouldPlayCorrectClip()
    {
        // Act
        audioManager.PlaySFX(1);

        // Assert
        Assert.AreEqual(testClip1, audioManager.currentSFXClip, "Expected the correct clip to be set in currentSFXClip");

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(gameObj);
        yield return null;
    }
}
