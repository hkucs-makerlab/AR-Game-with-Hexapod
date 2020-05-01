using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
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

        public enum ENEMY_TYPE {
            isArcher,
            isWeaponed,
            isPuncher
        }

        public enum GAME_PROGRESS {
            START_MENU,
            STORY,
            MAP_GENERATION,
            ROBOT_RECOGNITION,
            MOVING,
            FIGHTING,
            SHOPPING,
            WINNING
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

        public enum CHECKPOINT {
            DOOR,
            ENEMY
        }

        public static readonly float TIME_LIMIT = 10f;
        public static readonly float RESET_DELAY_TIME = 0.5f;
        public static readonly float FIGHT_OPERATION_TIME = 3f;
        public static readonly float PICK_UP_TIME = 1.5f;

        public static readonly List<Tuple<string, string, int, int>> SHOPPING_ITEMS = new List<Tuple<string, string, int, int>>() {
            Tuple.Create("ATK", "atk", 3, 10),
            Tuple.Create("DEF", "def", 3, 10),
            Tuple.Create("HP", "hp", 500, 10),
            Tuple.Create("Yellow Key", "yellowKey", 1, 10),
            Tuple.Create("Blue Key", "blueKey", 1, 30),
            Tuple.Create("Red Key", "redKey", 1, 50),
        };
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

    public static class SaveSystem {
        public static void SavePlayer(Player player) {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/Player.LocalSaving";
            FileStream stream = new FileStream(path, FileMode.Create);
            formatter.Serialize(stream, player);
            stream.Close();
        }

        public static Player LoadPlayerInfo() {
            string path = Application.persistentDataPath + "/Player.LocalSaving";
            if (File.Exists(path)) {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);
                Player player = (Player)formatter.Deserialize(stream);
                stream.Close();
                return player;
            }
            return null;
        }

        public static void SaveMap(Map map) {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/Map.LocalSaving";
            FileStream stream = new FileStream(path, FileMode.Create);
            formatter.Serialize(stream, map);
            stream.Close();
        }

        public static Map LoadMapInfo() {
            string path = Application.persistentDataPath + "/Map.LocalSaving";
            if (File.Exists(path)) {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);
                Map map = (Map)formatter.Deserialize(stream);
                stream.Close();
                return map;
            }
            return null;
        }

        public static void DeleteAllData() {
            File.Delete(Application.persistentDataPath + "/Player.LocalSaving");
            File.Delete(Application.persistentDataPath + "/Map.LocalSaving");
        }
    }
}
