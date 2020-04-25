using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HF_Static {
    public static class StaticData {
        public enum ITEM_TYPE {
            YELLOW_KEY,
            BLUE_KEY,
            RED_KEY,
            KEY_BOX,
            RED_GEM,
            BLUE_GEM,
            RED_BOTTLE,
            BLUE_BOTTLE,
            GOLD
        }

        public enum DOOR_TYPE {
            YELLOW_DOOR,
            BLUE_DOOR,
            RED_DOOR
        }

        public enum GAME_PROGRESS {
            START_MENU,
            MAP_GENERATION,
            ROBOT_RECOGNITION,
            MOVING,
            FIGHTING
        }

        public enum MODE_LETTER {
            W,
            D,
            F
        }

        public enum MODE_NUMBER {
            NULL_SHOULD_NOT_BE_USED,
            ONE,
            TWO,
            THREE,
            FOUR
        }

        public enum DPAD_LETTER {
            s,
            f,
            b,
            l,
            r,
            w
        }

        public enum INSTRUCTION {
            BUTTON_SHOOT,
            BUTTON_SKILL,
            BUTTON_DEFEND,
            JOYSTICK_UP,
            JOYSTICK_DOWN,
            JOYSTICK_LEFT,
            JOYSTICK_RIGHT
        }

        public static readonly float TIME_LIMIT = 10f;
        public static readonly float RESET_DELAY_TIME = 0.5f;
        public static readonly float FIGHT_OPERATION_TIME = 3f;
        public static readonly float PICK_UP_TIME = 1.5f;
    }

    public static class Randomize {
        public static System.Random random = new System.Random();

        public static List<StaticData.INSTRUCTION> RandomInstruction(int n) {
            Array values = Enum.GetValues(typeof(StaticData.INSTRUCTION));

            List<StaticData.INSTRUCTION> instructions = new List<StaticData.INSTRUCTION>();
            for (int i = 0; i < n - 1; i++) {
                StaticData.INSTRUCTION instruction = (StaticData.INSTRUCTION)values.GetValue(random.Next(values.Length));
                if (instructions.Count >= 1) {
                    while (instruction == instructions[instructions.Count - 1]) {
                        instruction = (StaticData.INSTRUCTION)values.GetValue(random.Next(values.Length));
                    }
                }
                instructions.Add(instruction);
            }

            StaticData.INSTRUCTION lastOperation = (StaticData.INSTRUCTION)values.GetValue(random.Next(3));
            instructions.Add(lastOperation);

            return instructions;
        }
    }
}
