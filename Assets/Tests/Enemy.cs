using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator enemyAnimator;

    public void TriggerAttack()
    {
        enemyAnimator.SetTrigger("FlipAttack");
    }

    public void TriggerDeath()
    {
        enemyAnimator.SetTrigger("FlipDeath");
    }
}
