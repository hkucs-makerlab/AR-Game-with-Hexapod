using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HF_Static;

public class MovementJoystick : Joystick {

    private StaticData.MODE_LETTER modeLetter;
    private StaticData.MODE_NUMBER modeNumber;
    private StaticData.DPAD_LETTER dPadLetter;

    private bool pickingUp;

    private void OnEnable() {
        Walk();
    }

    void Update()
    {
        if (pickingUp) {
            return;
        }

        StaticData.DPAD_LETTER newDPadLetter = StaticData.DPAD_LETTER.s; ;

        if (Horizontal != 0 || Vertical != 0) {
            if (Mathf.Abs(Horizontal) > Mathf.Abs(Vertical)) {
                newDPadLetter = Horizontal > 0 ? StaticData.DPAD_LETTER.r : StaticData.DPAD_LETTER.l;
            } else {
                newDPadLetter = Vertical > 0 ? StaticData.DPAD_LETTER.f : StaticData.DPAD_LETTER.b;
            }
        }

        if (newDPadLetter != dPadLetter) {
            dPadLetter = newDPadLetter;
            RobotController.Instance.SetMovementMode(modeLetter, modeNumber, dPadLetter);
        }
    }

    public void PickUp() {
        pickingUp = true;
        modeLetter = StaticData.MODE_LETTER.D;
        modeNumber = StaticData.MODE_NUMBER.THREE;
        dPadLetter = StaticData.DPAD_LETTER.w;
        RobotController.Instance.SetMovementMode(modeLetter, modeNumber, dPadLetter);
    }

    public void Run() {
        modeLetter = StaticData.MODE_LETTER.W;
        modeNumber = StaticData.MODE_NUMBER.FOUR;
        dPadLetter = StaticData.DPAD_LETTER.s;
        RobotController.Instance.SetMovementMode(modeLetter, modeNumber, dPadLetter);
    }

    public void Walk() {
        pickingUp = false;
        modeLetter = StaticData.MODE_LETTER.W;
        modeNumber = StaticData.MODE_NUMBER.ONE;
        dPadLetter = StaticData.DPAD_LETTER.s;
        RobotController.Instance.SetMovementMode(modeLetter, modeNumber, dPadLetter);
    }
}
