using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HF_Static;

[System.Serializable]
public class Player : System.Object
{
    int lv, hp, atk, def, money, exp, yellowKey, blueKey, redKey;

    public Player() {
        lv = 1;
        hp = 1000;
        atk = 10;
        def = 10;
        money = 0;
        exp = 0;
        yellowKey = 0;
        blueKey = 0;
        redKey = 0;
    }

    public void PickUp(StaticData.ITEM_TYPE type) {
        switch (type) {
            case StaticData.ITEM_TYPE.YELLOW_KEY:
                yellowKey++;
                break;
            case StaticData.ITEM_TYPE.BLUE_KEY:
                blueKey++;
                break;
            case StaticData.ITEM_TYPE.RED_KEY:
                redKey++;
                break;
            case StaticData.ITEM_TYPE.KEY_BOX:
                yellowKey++;
                blueKey++;
                redKey++;
                break;
            case StaticData.ITEM_TYPE.RED_GEM:
                atk += 3;
                break;
            case StaticData.ITEM_TYPE.BLUE_GEM:
                def += 3;
                break;
            case StaticData.ITEM_TYPE.RED_BOTTLE:
                hp += 200;
                break;
            case StaticData.ITEM_TYPE.BLUE_BOTTLE:
                hp += 500;
                break;
            case StaticData.ITEM_TYPE.GOLD:
                money += 200;
                break;
        }

        SaveSystem.SavePlayer(this);
    }

    public bool OpenDoor(StaticData.DOOR_TYPE type) {
        bool canOpen = false;
        switch (type) {
            case StaticData.DOOR_TYPE.YELLOW_DOOR:
                if (yellowKey > 0)
                {
                    yellowKey--;
                    canOpen = true;
                }
                break;
            case StaticData.DOOR_TYPE.BLUE_DOOR:
                if (blueKey > 0)
                {
                    blueKey--;
                    canOpen = true;
                }
                break;
            case StaticData.DOOR_TYPE.RED_DOOR:
                if (redKey > 0)
                {
                    redKey--;
                    canOpen = true;
                }
                break;
        }

        SaveSystem.SavePlayer(this);
        return canOpen;
    }

    public void LevelUp() {
        hp += 1000;
        atk += 7;
        def += 7;
    }

    public void BeingAttack(int atk, int multiplier) {
        int minus = atk * multiplier - def;
        minus = minus <= 0 ? 0 : minus;
        hp -= minus;
    }

    public void DefeatEnemy(int gold, int exp) {
        this.money += gold;
        this.exp += exp;
        SaveSystem.SavePlayer(this);
    }

    public int GetLv() {
        return lv;
    }

    public int GetHp()
    {
        return hp;
    }

    public int GetAtk()
    {
        return atk;
    }

    public int GetDef()
    {
        return def;
    }

    public int GetMoney()
    {
        return money;
    }

    public int GetExp()
    {
        return exp;
    }

    public int GetYellowKey()
    {
        return yellowKey;
    }

    public int GetBlueKey()
    {
        return blueKey;
    }

    public int GetRedKey()
    {
        return redKey;
    }
}
