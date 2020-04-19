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
    }

    public static class Randomize {
        public static System.Random random = new System.Random();
    }
}
