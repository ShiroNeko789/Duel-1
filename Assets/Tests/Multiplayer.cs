using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Multiplayer : MonoBehaviour
{
    public Text instructionText;
    public Text resultText;
    public Text player1ScoreText;
    public Text player2ScoreText;

    public Animator player1Animator;  // Animator for Player 1 (character animation)
    public Animator player2Animator;  // Animator for Player 2 (enemy animation)

    private float reactionTimePlayer1;
    private float reactionTimePlayer2;
    private bool isGameActive;
    private bool drawShown;
    private int scorePlayer1 = 0;
    private int scorePlayer2 = 0;
    private bool player1Attempted;
    private bool player2Attempted;
    
    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        instructionText.text = "Get Ready...";
        resultText.text = "";
        isGameActive = true;
        drawShown = false;
        player1Attempted = false;
        player2Attempted = false;
        reactionTimePlayer1 = 0;
        reactionTimePlayer2 = 0;

        StartCoroutine(ShowDrawSignal());
    }

    private IEnumerator ShowDrawSignal()
    {
        yield return new WaitForSeconds(Random.Range(1f, 4f));
        instructionText.text = "Duel!";
        audioManager.PlaySFX(audioManager.duel);
        drawShown = true;
    }

    private void Update()
    {
        // Player 1 input
        if (isGameActive && !drawShown && Input.GetKeyDown(KeyCode.S) && !player1Attempted)
        {
            EarlyPress(1);
        }
        else if (isGameActive && drawShown && Input.GetKeyDown(KeyCode.S) && !player1Attempted)
        {
            player1Attempted = true;
            player1Animator.SetTrigger("Attack");
            audioManager.PlaySFX(audioManager.attack);// Player 1's attack animation
            reactionTimePlayer1 = Time.time;  // Capture Player 1's reaction time
            CheckWinner();
        }

        // Player 2 input
        if (isGameActive && !drawShown && Input.GetKeyDown(KeyCode.L) && !player2Attempted)
        {
            EarlyPress(2);
        }
        else if (isGameActive && drawShown && Input.GetKeyDown(KeyCode.L) && !player2Attempted)
        {
            player2Attempted = true;
            player2Animator.SetTrigger("FAttack");
            audioManager.PlaySFX(audioManager.attack);// Player 2's attack animation
            reactionTimePlayer2 = Time.time;  // Capture Player 2's reaction time
            CheckWinner();
        }
    }

    private void CheckWinner()
    {
        // Check if both players attempted; if so, determine the faster one
        if (player1Attempted && player2Attempted)
        {
            float reactionDifference = reactionTimePlayer1 - reactionTimePlayer2;

            if (reactionDifference < 0)  // Player 1 was faster
            {
                scorePlayer1++;
                player1ScoreText.text = "Score: " + scorePlayer1;
                resultText.text = "Player 1 Wins!";
                audioManager.PlaySFX(audioManager.win);
                audioManager.PlaySFX(audioManager.death);

                Debug.Log("Player 2 FDeath Triggered");  // Debug log for Player 2's death
                player2Animator.SetTrigger("FDeath");  // Player 2's death animation
            }
            else  // Player 2 was faster
            {
                scorePlayer2++;
                player2ScoreText.text = "Score: " + scorePlayer2;
                resultText.text = "Player 2 Wins!";
                audioManager.PlaySFX(audioManager.lose);
                audioManager.PlaySFX(audioManager.death);
                Debug.Log("Player 1 Death Triggered");  // Debug log for Player 1's death
                player1Animator.SetTrigger("Death");  // Player 1's death animation
            }

            isGameActive = false;
            StartCoroutine(RestartDuelAfterDelay(3f));
        }
    }

    private void EarlyPress(int player)
    {
        resultText.text = player == 1 ? "Player 1 Rushed!" : "Player 2 Rushed!";

        if (player == 1)
        {
            player1Animator.SetTrigger("Death");
            audioManager.PlaySFX(audioManager.lose);// Player 1's early death animation
        }
        else
        {
            player2Animator.SetTrigger("FDeath");
            audioManager.PlaySFX(audioManager.lose);// Player 2's early death animation
        }

        isGameActive = false;
        StartCoroutine(RestartDuelAfterDelay(3f));
    }

    private IEnumerator RestartDuelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartGame();
    }
}
