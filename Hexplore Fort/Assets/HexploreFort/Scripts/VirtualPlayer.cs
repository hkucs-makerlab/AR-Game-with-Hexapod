using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HF_Static;

public class VirtualPlayer : MonoBehaviour
{
    public GameObject skill, defendShield;
    private Animator animator;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    public IEnumerator Shoot() {
        animator.SetBool("isShooting", true);

        yield return new WaitForSeconds(StaticData.FIGHT_OPERATION_TIME);

        animator.SetBool("isShooting", false);
    }

    public IEnumerator ActivateSkill() {
        skill.SetActive(true);

        yield return new WaitForSeconds(StaticData.FIGHT_OPERATION_TIME);

        skill.SetActive(false);
    }

    public IEnumerator Defend() {
        defendShield.SetActive(true);

        yield return new WaitForSeconds(StaticData.FIGHT_OPERATION_TIME);

        defendShield.SetActive(false);
    }

    public void EndFight() {
        animator.SetBool("isShooting", false);
        skill.SetActive(false);
        defendShield.SetActive(false);
    }
}
