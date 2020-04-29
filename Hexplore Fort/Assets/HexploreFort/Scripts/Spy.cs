using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HF_Static;

public class Spy : MonoBehaviour {
    private Animator animator;
    private VirtualPlayer playerObj;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    private void Update() {
        if (playerObj) {
            animator.SetBool("loop", false);
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f) {
                if (animator.GetBool("isTalking")) {
                    animator.SetBool("loop", true);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Player" && !playerObj) {
            playerObj = collision.gameObject.GetComponent<VirtualPlayer>();
            animator.SetBool("isTalking", true);
            GameManager.Instance.shoppingWindow.SetActive(true);
        }
    }

    private void OnCollisionExit(Collision collision) {
        if (collision.gameObject.tag == "Player") {
            playerObj = null;
            animator.SetBool("isTalking", false);
            GameManager.Instance.shoppingWindow.SetActive(false);
        }
    }
}
