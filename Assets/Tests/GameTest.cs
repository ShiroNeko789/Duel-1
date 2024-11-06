using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GameTest
{
    private GameObject gameObj;
    private QuickDrawGame quickDrawGame;
    private GameObject audioManagerObj;

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

    // Your other test methods here...

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(gameObj);
        Object.Destroy(audioManagerObj);
        yield return null;
    }
}
