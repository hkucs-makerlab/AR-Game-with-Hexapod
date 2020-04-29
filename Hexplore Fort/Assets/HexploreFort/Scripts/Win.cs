using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HF_Static;

public class Win : MonoBehaviour {
    private Animator animator;
    private VirtualPlayer playerObj;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    private void Update() {
        if (playerObj) {
            animator.SetBool("loop", false);
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f) {
                if (animator.GetBool("isCrying")) {
                    animator.SetBool("loop", true);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Player" && !playerObj) {
            playerObj = collision.gameObject.GetComponent<VirtualPlayer>();
            animator.SetBool("isCrying", true);
            GameManager.Instance.ChangeProgress(StaticData.GAME_PROGRESS.WINNING);
            Celebrate();
        }
    }

    private void Celebrate() {
        StaticData.MODE_LETTER modeLetter = StaticData.MODE_LETTER.D;
        StaticData.MODE_NUMBER modeNumber = StaticData.MODE_NUMBER.ONE;
        StaticData.DPAD_LETTER dPadLetter = StaticData.DPAD_LETTER.s;

        int random = Randomize.random.Next(4);
        switch (random) {
            case 0:
                modeNumber = StaticData.MODE_NUMBER.THREE;
                dPadLetter = StaticData.DPAD_LETTER.l;
                break;
            case 1:
                dPadLetter = StaticData.DPAD_LETTER.l;
                break;
            case 2:
                dPadLetter = StaticData.DPAD_LETTER.f;
                break;
            case 3:
                dPadLetter = StaticData.DPAD_LETTER.r;
                break;
        }
        RobotController.Instance.SetMovementMode(modeLetter, modeNumber, dPadLetter);
    }
}
