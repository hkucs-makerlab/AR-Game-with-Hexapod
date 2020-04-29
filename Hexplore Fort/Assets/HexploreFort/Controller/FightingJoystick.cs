using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HF_Static;

public class FightingJoystick : Joystick {
    [SerializeField]
    private GameObject instructionContainer, instructionResult;
    [SerializeField]
    private Slider timer, enemyHp;
    [SerializeField]
    private Text hp, atk, def;

    private StaticData.MODE_LETTER modeLetter;
    private StaticData.MODE_NUMBER modeNumber;
    private StaticData.DPAD_LETTER dPadLetter;

    private Dictionary<StaticData.INSTRUCTION, Sprite> instructionIcons;
    private List<StaticData.INSTRUCTION> instructions;
    private float timeRemain;
    private bool operating;
    private int noOfInstruction, noOfPressed, fullHp, combo;

    private Fight fight;
    private StaticData.INSTRUCTION operation;

    private void Awake() {
        instructionIcons = new Dictionary<StaticData.INSTRUCTION, Sprite>();
        foreach (StaticData.INSTRUCTION instruction in Enum.GetValues(typeof(StaticData.INSTRUCTION))) {
            Sprite icon = Resources.Load<Sprite>("Textures/" + instruction.ToString());
            instructionIcons.Add(instruction, icon);
        }
    }

    private void OnEnable() {
        modeLetter = StaticData.MODE_LETTER.W;
        modeNumber = StaticData.MODE_NUMBER.ONE;
        dPadLetter = StaticData.DPAD_LETTER.s;
        RobotController.Instance.SetMovementMode(modeLetter, modeNumber, dPadLetter);
    }

    private void OnDisable() {
        operating = false;
        fight = null;
        combo = 0;

        ResetInput();
    }

    public void SetNoOfInstruction(Fight fight) {
        noOfInstruction = fight.enemy.GetLv();
        this.fight = fight;
        fullHp = this.fight.enemy.GetHp();
        Attack();
        NewInstructions();
        instructionContainer.transform.parent.gameObject.SetActive(true);
    }

    private void Update() {
        if (fight) {
            enemyHp.value = (float)fight.enemy.GetHp() / (float)fullHp;
            hp.text = fight.enemy.GetHp() + "/" + fullHp;
            atk.text = fight.enemy.GetAtk() + "";
            def.text = fight.enemy.GetDef() + "";
        }

        if (fight && fight.enemy.GetHp() <= 0) {
            return;
        }

        if (operating) {
            return;
        }

        timeRemain -= Time.deltaTime;
        timer.value = timeRemain / StaticData.TIME_LIMIT;
        if (timeRemain <= 0) {
            instructionResult.GetComponent<Text>().text = "MISS";
            instructionResult.GetComponent<Animation>().Play("InstructionMiss");

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
                child.GetChild(0).gameObject.GetComponent<Image>().sprite = instructionIcons[instructions[counting]];
                child.gameObject.name = instructions[counting].ToString();
            } else {
                child.gameObject.SetActive(false);
            }
            counting++;
        }
        operation = instructions[instructions.Count - 1];
    }

    private void PressedCorrectly() {
        if (fight && fight.enemy.GetHp() <= 0) {
            return;
        }

        instructionContainer.transform.GetChild(noOfPressed).gameObject.SetActive(false);
        noOfPressed++;
    }

    private void Attack() {
        if (fight && fight.enemy.GetHp() <= 0) {
            return;
        }

        modeLetter = StaticData.MODE_LETTER.D;
        modeNumber = StaticData.MODE_NUMBER.ONE;
        dPadLetter = StaticData.DPAD_LETTER.w;
        RobotController.Instance.SetMovementMode(modeLetter, modeNumber, dPadLetter);
    }

    public void Shoot() {
        if (fight && fight.enemy.GetHp() <= 0) {
            return;
        }

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
        if (fight && fight.enemy.GetHp() <= 0) {
            return;
        }

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
        if (fight && fight.enemy.GetHp() <= 0) {
            return;
        }

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

        StartCoroutine(Stand(false));
    }

    private IEnumerator Stand(bool showResult = true) {
        operating = true;
        if (showResult) {
            combo++;
            instructionResult.GetComponent<Text>().text = "COMBO x" + combo;
            instructionResult.GetComponent<Animation>().Play("InstructionPerfect");
        }

        yield return new WaitForSeconds(StaticData.FIGHT_OPERATION_TIME);

        modeLetter = StaticData.MODE_LETTER.W;
        modeNumber = StaticData.MODE_NUMBER.ONE;
        dPadLetter = StaticData.DPAD_LETTER.s;
        RobotController.Instance.SetMovementMode(modeLetter, modeNumber, dPadLetter);

        if (!showResult) {
            yield return new WaitForSeconds(StaticData.RESET_DELAY_TIME);

            operating = false;
            Attack();
            NewInstructions();
        }
    }
}
