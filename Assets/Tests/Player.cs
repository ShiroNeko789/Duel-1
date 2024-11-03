using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator playerAnimator;

    public void PlayAttackAnimation()
    {
        playerAnimator.SetTrigger("Attack");
    }

    public void PlayDeathAnimation()
    {
        playerAnimator.SetTrigger("Death");
    }
}
