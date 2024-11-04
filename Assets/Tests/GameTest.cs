using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using UnityEngine.UI;

public class GameTest
{
    private GameObject gameObject;
    private QuickDrawGame quickDrawGame;

    [SetUp]
    public void Setup()
    {
        // Create main GameObject for QuickDrawGame
        gameObject = new GameObject();
        quickDrawGame = gameObject.AddComponent<QuickDrawGame>();

        // Initialize and assign Text components for UI
        quickDrawGame.instructionText = new GameObject().AddComponent<Text>();
        quickDrawGame.resultText = new GameObject().AddComponent<Text>();
        quickDrawGame.scoreText = new GameObject().AddComponent<Text>();

        // Create a character GameObject and assign Animator
        GameObject characterObject = new GameObject();
        quickDrawGame.characterAnimator = characterObject.AddComponent<Animator>();
        quickDrawGame.characterAnimator.runtimeAnimatorController =
            Resources.Load<RuntimeAnimatorController>("EnemyAnimatorController"); // Load character animator

        // Create an enemy GameObject and assign Enemy controller
        GameObject enemyObject = new GameObject();
        quickDrawGame.enemyController = enemyObject.AddComponent<Enemy>();
        quickDrawGame.enemyController.enemyAnimator = enemyObject.AddComponent<Animator>();
        quickDrawGame.enemyController.enemyAnimator.runtimeAnimatorController =
            Resources.Load<RuntimeAnimatorController>("EnemyAnimatorController"); // Load enemy animator

        quickDrawGame.characterAnimator = new GameObject().AddComponent<Animator>();
        quickDrawGame.enemyController = new GameObject().AddComponent<Enemy>(); // Ensure you have an Enemy script
        quickDrawGame.instructionText = gameObject.AddComponent<Text>();
        quickDrawGame.resultText = gameObject.AddComponent<Text>();
        quickDrawGame.scoreText = gameObject.AddComponent<Text>();

        // Mock or setup additional components as needed
        AudioManager.instance = new GameObject().AddComponent<AudioManager>();

        // Start the game to initialize all game elements
        quickDrawGame.StartGame();
    }

    [UnityTest]
    public IEnumerator GameInitializationTest()
    {
        // Ensure that game initializes without unhandled errors
        Assert.IsTrue(quickDrawGame.isGameActive, "Game should be active after initialization.");
        yield return null;
    }

    [UnityTest]
    public IEnumerator ScoreIncrementsOnWinTest()
    {
        // Arrange
        quickDrawGame.score = 0; // Start with a score of 0
        quickDrawGame.isGameActive = true;
        quickDrawGame.drawShown = true; // Assume the duel is already on
        quickDrawGame.hasAttempted = true; // Assume player has pressed space
        float playerReactionTime = 0.2f; // Set a valid reaction time for a win

        // Make sure enemyController is not null
        Assert.IsNotNull(quickDrawGame.enemyController, "EnemyController should not be null.");

        // Act
        quickDrawGame.CheckWinOrLose(playerReactionTime);

        // Assert
        Assert.AreEqual(1, quickDrawGame.score, "Score should be incremented to 1.");
        Assert.AreEqual("Player Win!", quickDrawGame.resultText.text, "Result text should indicate a player win.");
        yield return null;
    }

    [UnityTest]
    public IEnumerator DuelLossTest()
    {
        // Set the duel to be active and simulate the player's early press
        quickDrawGame.isGameActive = true;
        quickDrawGame.drawShown = false; // Ensure "Duel!" has not been shown
        quickDrawGame.hasAttempted = false; // Reset attempted flag for the test

        // Simulate an early space press
        quickDrawGame.Update(); // This will check for early space presses

        // Directly simulate the early press
        if (Input.GetKeyDown(KeyCode.Space)) // We need to mimic the key press
        {
            quickDrawGame.Update(); // Call the update method again to process the input
        }
        else
        {
            // Simulate the early press directly by calling the method if Input isn't detected
            quickDrawGame.Update(); // This should process the early space press
        }

        // Assert that the result text indicates loss
        Assert.AreEqual("Rush For What?!", quickDrawGame.resultText.text);
        Assert.IsTrue(quickDrawGame.characterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Death"), "Character should be in the death state.");
        yield return null;
    }

    [UnityTest]
    public IEnumerator EarlySpacePressTest()
    {
        // Set up the game state
        quickDrawGame.isGameActive = true;
        quickDrawGame.drawShown = false; // Ensure "Duel!" has not been shown
        quickDrawGame.hasAttempted = false; // Reset attempted flag for the test

        // Directly simulate the early space press
        quickDrawGame.Update(); // Call to check for early press (should do nothing as drawShown is false)

        // Simulate pressing the space key too early
        quickDrawGame.CheckEarlyPress(); // Call a helper function to check for early press

        // Assert that the result text indicates loss
        Assert.AreEqual("Rush For What?!", quickDrawGame.resultText.text);
        Assert.IsTrue(quickDrawGame.characterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Death"), "Character should be in the death state.");
        yield return null;
    }
}
