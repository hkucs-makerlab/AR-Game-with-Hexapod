using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HF_Static;

public class FightingJoystick : Joystick {

    private StaticData.MODE_LETTER modeLetter;
    private StaticData.MODE_NUMBER modeNumber;
    private StaticData.DPAD_LETTER dPadLetter;

    private void OnEnable() {
        Attack();
    }

    void Update() {
        if (Horizontal != 0 || Vertical != 0) {
            if (Mathf.Abs(Horizontal) > Mathf.Abs(Vertical)) {
                //newDPadLetter = Horizontal > 0 ? StaticData.DPAD_LETTER.r : StaticData.DPAD_LETTER.l;
            } else {
                //newDPadLetter = Vertical > 0 ? StaticData.DPAD_LETTER.f : StaticData.DPAD_LETTER.b;
            }
        }
    }

    private void Attack() {
        modeLetter = StaticData.MODE_LETTER.D;
        modeNumber = StaticData.MODE_NUMBER.ONE;
        dPadLetter = StaticData.DPAD_LETTER.w;
        RobotController.Instance.SetMovementMode(modeLetter, modeNumber, dPadLetter);
    }

    public void Shoot() {
        modeLetter = StaticData.MODE_LETTER.D;
        modeNumber = StaticData.MODE_NUMBER.FOUR;
        dPadLetter = StaticData.DPAD_LETTER.f;
        RobotController.Instance.SetMovementMode(modeLetter, modeNumber, dPadLetter);

        Invoke("Attack", 2f);
    }

    public void Skill() {
        modeLetter = StaticData.MODE_LETTER.F;
        modeNumber = StaticData.MODE_NUMBER.ONE;
        dPadLetter = StaticData.DPAD_LETTER.w;
        RobotController.Instance.SetMovementMode(modeLetter, modeNumber, dPadLetter);

        Invoke("Attack", 2f);
    }

    public void Defend() {
        modeLetter = StaticData.MODE_LETTER.F;
        modeNumber = StaticData.MODE_NUMBER.THREE;
        dPadLetter = StaticData.DPAD_LETTER.b;
        RobotController.Instance.SetMovementMode(modeLetter, modeNumber, dPadLetter);

        Invoke("Attack", 2f);
    }

    public void Celebrate() {
        modeLetter = StaticData.MODE_LETTER.D;
        int random = Randomize.random.Next(4);
        switch (random) {
            case 0: 
                modeNumber = StaticData.MODE_NUMBER.THREE;
                dPadLetter = StaticData.DPAD_LETTER.l;
                break;
            case 1:
                modeNumber = StaticData.MODE_NUMBER.ONE;
                dPadLetter = StaticData.DPAD_LETTER.l;
                break;
            case 2:
                modeNumber = StaticData.MODE_NUMBER.ONE;
                dPadLetter = StaticData.DPAD_LETTER.f;
                break;
            case 3:
                modeNumber = StaticData.MODE_NUMBER.ONE;
                dPadLetter = StaticData.DPAD_LETTER.r;
                break;
        }
        RobotController.Instance.SetMovementMode(modeLetter, modeNumber, dPadLetter);

        Invoke("Attack", 2f);
    }
}
