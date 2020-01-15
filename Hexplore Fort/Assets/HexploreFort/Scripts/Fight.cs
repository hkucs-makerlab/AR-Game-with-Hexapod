using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight : MonoBehaviour
{
    public Animator animator;
    private GameObject player;

    private void Update()
    {
        animator.SetBool("loop", false);
        if (player)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f)
            {
                if (animator.GetBool("isFighting"))
                {
                    animator.SetBool("loop", true);
                }
                else if (animator.GetBool("isReady"))
                {
                    animator.SetBool("isFighting", true);
                }

                if (Vector3.Distance(player.transform.position, transform.position) > 6)
                {
                    animator.SetBool("isReady", false);
                    animator.SetBool("isFighting", false);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player") {
            player = collision.gameObject;
            animator.SetBool("isReady", true);
        }
    }
}
