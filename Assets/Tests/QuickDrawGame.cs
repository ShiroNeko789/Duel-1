using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickDrawGame : MonoBehaviour
{
    public Text instructionText;  // UI for game instructions
    public Text resultText;       // UI for win/lose result
    public Text scoreText;        // UI for player score

    public float reactionTime;   // Player's reaction time
    public bool isGameActive;    // True when a duel is active
    private bool drawShown;       // True when "Draw!" appears
    public int score = 0;        // Player's score
    public Animator characterAnimator;
    public Enemy enemyController; // Reference to the EnemyController

    void Start()
    {
        // Check if characterAnimator has a controller, assign one if missing for test purposes
        if (characterAnimator.runtimeAnimatorController == null)
        {
            characterAnimator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("EnemyAnimatorController");
        }

        // Ensure enemy animator also has a controller if missing
        if (enemyController.enemyAnimator.runtimeAnimatorController == null)
        {
            enemyController.enemyAnimator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("EnemyAnimatorController");
        }

        StartGame();
    }

    public void StartGame()
    {
        score = 0;
        UpdateScore();
        instructionText.text = "Get Ready...";
        resultText.text = "";
        isGameActive = true;
        drawShown = false;

        // Start the countdown for "Draw!"
        StartCoroutine(ShowDrawSignal());
    }

    private IEnumerator ShowDrawSignal()
    {
        yield return new WaitForSeconds(Random.Range(2f, 5f));
        instructionText.text = "Duel!";
        drawShown = true;
        reactionTime = Time.time;  // Capture the time when "Draw!" appears

        // Enemy automatically attacks when "Duel!" appears
        enemyController.TriggerAttack();
    }

    void Update()
    {
        if (isGameActive && drawShown && Input.GetKeyDown(KeyCode.Space))
        {
            characterAnimator.SetTrigger("Attack");

            float playerReactionTime = Time.time - reactionTime;
            CheckWinOrLose(playerReactionTime);
        }
    }

    public void CheckWinOrLose(float playerReactionTime)
    {
        // Simulate NPC reaction time (0.1 to 0.6 seconds)
        float npcReactionTime = Random.Range(0.1f, 0.6f);

        if (playerReactionTime < npcReactionTime)
        {
            score++;
            resultText.text = "Player Win!";
            enemyController.TriggerDeath(); // Enemy dies if player wins
            StartCoroutine(RestartDuelAfterDelay(6f)); // Wait 6 seconds before restarting when player wins
            return; // Exit to prevent immediate restart
        }
        else
        {
            resultText.text = "Enemy Win!";
            characterAnimator.SetTrigger("FDeath"); // Player dies if player loses
            StartCoroutine(RestartDuelAfterDelay(6f)); // Wait 5 seconds before restarting when player loses
            return; // Exit to prevent immediate restart
        }
    }

    private void UpdateScore()
    {
        scoreText.text = "Win: " + score;
    }

    private void RestartDuel()
    {
        drawShown = false;
        instructionText.text = "Get Ready...";
        StartCoroutine(ShowDrawSignal());
    }

    private IEnumerator RestartDuelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        RestartDuel();
    }
}
