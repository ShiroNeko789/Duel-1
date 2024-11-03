using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GameTest
{
    private GameObject gameObj;
    private QuickDrawGame quickDrawGame;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Create GameObject and attach QuickDrawGame
        gameObj = new GameObject();
        quickDrawGame = gameObj.AddComponent<QuickDrawGame>();

        // Set up UI Texts
        quickDrawGame.instructionText = new GameObject().AddComponent<UnityEngine.UI.Text>();
        quickDrawGame.resultText = new GameObject().AddComponent<UnityEngine.UI.Text>();
        quickDrawGame.scoreText = new GameObject().AddComponent<UnityEngine.UI.Text>();

        // Set up Animators with controller assignment
        var characterObj = new GameObject("CharacterAnimator");
        quickDrawGame.characterAnimator = characterObj.AddComponent<Animator>();
        quickDrawGame.characterAnimator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("EnemyAnimatorController");

        var enemyObj = new GameObject("Enemy");
        var enemyController = enemyObj.AddComponent<Enemy>();
        enemyController.enemyAnimator = enemyObj.AddComponent<Animator>();
        enemyController.enemyAnimator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("EnemyAnimatorController");
        quickDrawGame.enemyController = enemyController;

        // Wait for the Animator to initialize
        yield return null;
    }

    [UnityTest]
    public IEnumerator StartGame_InitializesGameCorrectly()
    {
        // Act
        quickDrawGame.StartGame();

        // Assert initial state
        Assert.AreEqual("Get Ready...", quickDrawGame.instructionText.text);
        Assert.AreEqual("", quickDrawGame.resultText.text);
        Assert.AreEqual(0, quickDrawGame.score);

        yield return null;
    }

    [UnityTest]
    public IEnumerator CheckWinOrLose_PlayerWinsInTime_IncrementsScore()
    {
        // Arrange
        quickDrawGame.StartGame();
        float playerReactionTime = 0.2f;

        // Act
        quickDrawGame.CheckWinOrLose(playerReactionTime);

        // Assert
        Assert.AreEqual(1, quickDrawGame.score);
        Assert.AreEqual("Player Win!", quickDrawGame.resultText.text);

        yield return null;
    }

    [UnityTest]
    public IEnumerator CheckWinOrLose_PlayerLosesToNPC_TriggerDeathAnimation()
    {
        // Arrange
        quickDrawGame.StartGame();
        float playerReactionTime = 0.7f;

        // Act
        quickDrawGame.CheckWinOrLose(playerReactionTime);

        // Assert
        Assert.AreEqual("Enemy Win!", quickDrawGame.resultText.text);

        // Wait for 6 seconds as per game behavior for reset
        yield return new WaitForSeconds(6f);

        // Assert game reset
        Assert.AreEqual("Get Ready...", quickDrawGame.instructionText.text);
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(gameObj);
        yield return null;
    }
}
