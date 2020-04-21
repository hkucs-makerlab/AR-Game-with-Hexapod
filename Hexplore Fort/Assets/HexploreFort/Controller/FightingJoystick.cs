using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HF_Static;

public class FightingJoystick : Joystick {
    public GameObject instructionContainer;
    public Slider timer;

    private StaticData.MODE_LETTER modeLetter;
    private StaticData.MODE_NUMBER modeNumber;
    private StaticData.DPAD_LETTER dPadLetter;

    private List<StaticData.INSTRUCTION> instructions;
    private float timeRemain;
    private bool operating;
    private int noOfInstruction, noOfPressed;

    private Fight fight;
    public StaticData.INSTRUCTION operation;

    private void OnEnable() {
        modeLetter = StaticData.MODE_LETTER.W;
        modeNumber = StaticData.MODE_NUMBER.ONE;
        dPadLetter = StaticData.DPAD_LETTER.s;
        RobotController.Instance.SetMovementMode(modeLetter, modeNumber, dPadLetter);
    }

    private void OnDisable() {
        operating = false;

        ResetInput();
    }

    public void SetNoOfInstruction(Fight fight) {
        noOfInstruction = fight.enemy.GetLv();
        this.fight = fight;
        Attack();
        NewInstructions();
    }

    private void Update() {
        if (fight && fight.enemy.GetHp() <= 0) {
            return;
        }

        if (operating) {
            return;
        }

        timeRemain -= Time.deltaTime;
        timer.value = timeRemain / StaticData.TIME_LIMIT;
        if (timeRemain <= 0) {
            if (operation == StaticData.INSTRUCTION.BUTTON_DEFEND) {
                fight.MissDefendOperation();
            }
            NewInstructions();
        }

        if (noOfPressed < noOfInstruction) {
            if (Horizontal != 0 || Vertical != 0) {
                StaticData.INSTRUCTION pressedInstruction;
                if (Mathf.Abs(Horizontal) > Mathf.Abs(Vertical)) {
                    pressedInstruction = Horizontal > 0 ? StaticData.INSTRUCTION.JOYSTICK_RIGHT : StaticData.INSTRUCTION.JOYSTICK_LEFT;
                } else {
                    pressedInstruction = Vertical > 0 ? StaticData.INSTRUCTION.JOYSTICK_UP : StaticData.INSTRUCTION.JOYSTICK_DOWN;
                }

                if (pressedInstruction == instructions[noOfPressed]) {
                    PressedCorrectly();
                }
            }
        }
    }

    private void NewInstructions() {
        timeRemain = StaticData.TIME_LIMIT;
        instructions = Randomize.RandomInstruction(noOfInstruction);
        noOfPressed = 0;
        int counting = 0;
        foreach (Transform child in instructionContainer.transform) {
            if (counting < instructions.Count) {
                child.gameObject.SetActive(true);
                child.gameObject.name = instructions[counting].ToString();
            } else {
                child.gameObject.SetActive(false);
            }
            counting++;
        }
        operation = instructions[instructions.Count - 1];
    }

    private void PressedCorrectly() {
        instructionContainer.transform.GetChild(noOfPressed).gameObject.SetActive(false);
        noOfPressed++;
    }

    private void Attack() {
        modeLetter = StaticData.MODE_LETTER.D;
        modeNumber = StaticData.MODE_NUMBER.ONE;
        dPadLetter = StaticData.DPAD_LETTER.w;
        RobotController.Instance.SetMovementMode(modeLetter, modeNumber, dPadLetter);
    }

    public void Shoot() {
        if (noOfPressed < noOfInstruction) {
            if (instructions[noOfPressed] == StaticData.INSTRUCTION.BUTTON_SHOOT) {
                PressedCorrectly();
                if (noOfPressed == noOfInstruction) {
                    fight.PlayerShoot();
                    modeLetter = StaticData.MODE_LETTER.D;
                    modeNumber = StaticData.MODE_NUMBER.FOUR;
                    dPadLetter = StaticData.DPAD_LETTER.f;
                    RobotController.Instance.SetMovementMode(modeLetter, modeNumber, dPadLetter);

                    StartCoroutine(Stand());
                }
            }
        }
    }

    public void Skill() {
        if (noOfPressed < noOfInstruction) {
            if (instructions[noOfPressed] == StaticData.INSTRUCTION.BUTTON_SKILL) {
                PressedCorrectly();
                if (noOfPressed == noOfInstruction) {
                    fight.PlayerActivateSkill();
                    modeLetter = StaticData.MODE_LETTER.F;
                    modeNumber = StaticData.MODE_NUMBER.ONE;
                    dPadLetter = StaticData.DPAD_LETTER.w;
                    RobotController.Instance.SetMovementMode(modeLetter, modeNumber, dPadLetter);

                    StartCoroutine(Stand());
                }
            }
        }
    }

    public void Defend() {
        if (noOfPressed < noOfInstruction) {
            if (instructions[noOfPressed] == StaticData.INSTRUCTION.BUTTON_DEFEND) {
                PressedCorrectly();
                if (noOfPressed == noOfInstruction) {
                    fight.PlayerDefend();
                    modeLetter = StaticData.MODE_LETTER.F;
                    modeNumber = StaticData.MODE_NUMBER.THREE;
                    dPadLetter = StaticData.DPAD_LETTER.b;
                    RobotController.Instance.SetMovementMode(modeLetter, modeNumber, dPadLetter);

                    StartCoroutine(Stand());
                }
            }
        }
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

        StartCoroutine(Stand());
    }

    private IEnumerator Stand() {
        operating = true;

        yield return new WaitForSeconds(StaticData.FIGHT_OPERATION_TIME);

        modeLetter = StaticData.MODE_LETTER.W;
        modeNumber = StaticData.MODE_NUMBER.ONE;
        dPadLetter = StaticData.DPAD_LETTER.s;
        RobotController.Instance.SetMovementMode(modeLetter, modeNumber, dPadLetter);

        yield return new WaitForSeconds(StaticData.RESET_DELAY_TIME);

        operating = false;
        Attack();
        NewInstructions();
    }
}
