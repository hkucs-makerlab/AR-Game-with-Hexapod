using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HF_Static;

public class MovementJoystick : Joystick {

    private StaticData.MODE_LETTER modeLetter;
    private StaticData.MODE_NUMBER modeNumber;
    private StaticData.DPAD_LETTER dPadLetter;

    private bool operating;
    public bool walking, pickingUp, running;

    private void OnEnable() {
        Walk();
    }

    private void OnDisable() {
        operating = false;
        walking = false;
        pickingUp = false;
        running = false;

        ResetInput();
    }

    void Update()
    {
        if (operating) {
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
        modeLetter = StaticData.MODE_LETTER.D;
        modeNumber = StaticData.MODE_NUMBER.THREE;
        dPadLetter = StaticData.DPAD_LETTER.w;
        RobotController.Instance.SetMovementMode(modeLetter, modeNumber, dPadLetter);

        StartCoroutine(Stand());
    }

    public void Run() {
        running = true;
        walking = false;
        modeLetter = StaticData.MODE_LETTER.W;
        modeNumber = StaticData.MODE_NUMBER.FOUR;
        dPadLetter = StaticData.DPAD_LETTER.s;
        RobotController.Instance.SetMovementMode(modeLetter, modeNumber, dPadLetter);
    }

    public void Walk() {
        pickingUp = false;
        running = false;
        walking = true;
        modeLetter = StaticData.MODE_LETTER.W;
        modeNumber = StaticData.MODE_NUMBER.ONE;
        dPadLetter = StaticData.DPAD_LETTER.s;
        RobotController.Instance.SetMovementMode(modeLetter, modeNumber, dPadLetter);
    }

    private IEnumerator Stand() {
        operating = true;
        walking = false;

        yield return new WaitForSeconds(StaticData.PICK_UP_TIME);

        pickingUp = true;

        yield return new WaitForSeconds(StaticData.RESET_DELAY_TIME);

        operating = false;
        Walk();
    }
}
