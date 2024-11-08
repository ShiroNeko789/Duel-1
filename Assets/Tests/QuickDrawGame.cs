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
    private bool hasAttempted;    // Flag to check if player has pressed space

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

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
        hasAttempted = false; // Reset the attempt flag

        // Start the countdown for "Draw!"
        StartCoroutine(ShowDrawSignal());
    }

    private IEnumerator ShowDrawSignal()
    {
        yield return new WaitForSeconds(Random.Range(1f, 4f));
        instructionText.text = "Duel!";
        drawShown = true;
        reactionTime = Time.time;  // Capture the time when "Draw!" appears

        // Enemy automatically attacks when "Duel!" appears
        enemyController.TriggerAttack();
    }

    void Update()
    {
        // Check if the player presses space too early
        if (isGameActive && Input.GetKeyDown(KeyCode.Space) && !drawShown && !hasAttempted)
        {
            hasAttempted = true; // Mark that an attempt was made

            // Trigger player's "death" animation and play the loss sound effect
            resultText.text = "Rush For What?!";
            characterAnimator.SetTrigger("Death");
            audioManager.PlaySFX(audioManager.lose);

            // Restart duel after a 3-second delay for early press penalty
            StartCoroutine(RestartDuelAfterDelay(1f));
        }
        else if (isGameActive && drawShown && Input.GetKeyDown(KeyCode.Space) && !hasAttempted)
        {
            hasAttempted = true; // Mark that the player has attempted to attack
            characterAnimator.SetTrigger("Attack");
            audioManager.PlaySFX(audioManager.attack);

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
            UpdateScore();
            Debug.Log("Score incremented to: " + score);
            resultText.text = "Player Win!";
            enemyController.TriggerDeath();
            audioManager.PlaySFX(audioManager.death);
            audioManager.PlaySFX(audioManager.win);
            StartCoroutine(RestartDuelAfterDelay(3f)); // Wait 6 seconds before restarting when player wins
        }
        else
        {
            resultText.text = "Enemy Win!";
            characterAnimator.SetTrigger("Death");
            audioManager.PlaySFX(audioManager.death);
            audioManager.PlaySFX(audioManager.lose);
            StartCoroutine(RestartDuelAfterDelay(3f)); // Wait 6 seconds before restarting when player loses
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
        resultText.text = "";  // Clear the result text
        hasAttempted = false; // Reset the attempt flag for the next duel
        StartCoroutine(ShowDrawSignal());
    }

    private IEnumerator RestartDuelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        RestartDuel();
    }
}