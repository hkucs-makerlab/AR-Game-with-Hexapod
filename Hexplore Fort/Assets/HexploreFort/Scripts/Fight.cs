using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight : MonoBehaviour {
    public Enemy enemy = new Enemy();

    private FightingJoystick joystick;
    private Animator animator;
    private Player player;
    private VirtualPlayer playerObj;
    private int enemyMultiplier, playerMultiplier;

    private void Start() {
        joystick = GameManager.Instance.fightingJoystick;

        animator = GetComponent<Animator>();
        player = GameManager.Instance.player;
        ResetEnemyMultiplier();
        ResetPlayerMultiplier();
    }

    private void Update() {
        if (playerObj)
        {
            animator.SetBool("loop", false);
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f)
            {
                if (animator.GetBool("isFighting"))
                {
                    animator.SetBool("loop", true);
                }
                else if (animator.GetBool("isReady"))
                {
                    animator.SetBool("isFighting", true);
                    StartCoroutine(Fighting());
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player") {
            playerObj = collision.gameObject.GetComponent<VirtualPlayer>();
            animator.SetBool("isReady", true);
            GameManager.Instance.SwitchFighting(true);
            joystick.SetNoOfInstruction(this);
        }
    }

    private IEnumerator Fighting() {
        while (enemy.GetHp() > 0) {
            player.BeingAttack(enemy.GetAtk(), enemyMultiplier);
            enemy.BeingAttack(player.GetAtk(), playerMultiplier);
            yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(2f);

        CancelInvoke();
        playerObj.EndFight();
        GameManager.Instance.SwitchFighting(false);
        Destroy(gameObject);
    }

    public void MissDefendOperation() {
        enemyMultiplier = 2;
        //enemy skill effect
        Invoke("ResetEnemyMultiplier", 3f);
    }

    public void PlayerShoot() {
        playerMultiplier = 2;
        StartCoroutine(playerObj.Shoot());
        Invoke("ResetPlayerMultiplier", 3f);
    }

    public void PlayerActivateSkill() {
        playerMultiplier = 2;
        StartCoroutine(playerObj.ActivateSkill());
        Invoke("ResetPlayerMultiplier", 3f);
    }

    public void PlayerDefend() {
        enemyMultiplier = 0;
        StartCoroutine(playerObj.Defend());
        Invoke("ResetEnemyMultiplier", 3f);
    }

    private void ResetEnemyMultiplier() {
        enemyMultiplier = 1;
    }

    private void ResetPlayerMultiplier() {
        playerMultiplier = 1;
    }
}
