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
        money = 10;
        exp = 0;
        yellowKey = 0;
        blueKey = 0;
        redKey = 0;
    }

    public void PickUp(StaticData.ITEM_TYPE type) {
        switch (type) {
            case StaticData.ITEM_TYPE.YELLOW_KEY:
                yellowKey++;
                GameManager.Instance.PopupMessage("Yellow Key +1");
                break;
            case StaticData.ITEM_TYPE.BLUE_KEY:
                blueKey++;
                GameManager.Instance.PopupMessage("Blue Key +1");
                break;
            case StaticData.ITEM_TYPE.RED_KEY:
                redKey++;
                GameManager.Instance.PopupMessage("Red Key +1");
                break;
            case StaticData.ITEM_TYPE.KEY_BOX:
                yellowKey++;
                blueKey++;
                redKey++;
                GameManager.Instance.PopupMessage("Yellow Key, Blue Key and Red Key +1");
                break;
            case StaticData.ITEM_TYPE.RED_GEM:
                atk += 3;
                GameManager.Instance.PopupMessage("ATK +3");
                break;
            case StaticData.ITEM_TYPE.BLUE_GEM:
                def += 3;
                GameManager.Instance.PopupMessage("DEF +3");
                break;
            case StaticData.ITEM_TYPE.RED_BOTTLE:
                hp += 200;
                GameManager.Instance.PopupMessage("HP +200");
                break;
            case StaticData.ITEM_TYPE.BLUE_BOTTLE:
                hp += 500;
                GameManager.Instance.PopupMessage("HP +500");
                break;
            case StaticData.ITEM_TYPE.GOLD:
                money += 200;
                GameManager.Instance.PopupMessage("Money +200");
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

    public void BeingAttack(int atk, int multiplier) {
        int minus = atk * multiplier - def;
        minus = minus <= 0 ? 0 : minus;
        hp -= minus;
    }

    public void DefeatEnemy(int gold, int exp) {
        this.money += gold;
        this.exp += exp;
        if (exp >= 100) {
            LevelUp();
            exp -= 100;
        }
        SaveSystem.SavePlayer(this);
    }

    private void LevelUp() {
        hp += 1000;
        atk += 7;
        def += 7;
    }

    public bool Shop(string attribute, int value, int price) {
        if (money < price) {
            return false;
        }

        switch (attribute) {
            case "yellowKey":
                yellowKey += value;
                break;
            case "blueKey":
                blueKey += value;
                break;
            case "redKey":
                redKey += value;
                break;
            case "atk":
                atk += value;
                break;
            case "def":
                def += value;
                break;
            case "hp":
                hp += value;
                break;
        }
        money -= price;
        return true;
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
