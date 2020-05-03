using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HF_Static;

[System.Serializable]
public class Enemy : System.Object
{
    [SerializeField]
    int lv, hp, atk, def, money, exp;
    [SerializeField]
    Sprite icon;

    public Enemy() {
        lv = 1;
        hp = 1000;
        atk = 11;
        def = 9;
        money = 1;
        exp = 1;
    }

    public void BeingAttack(int atk, int multiplier) {
        int minus = atk * multiplier - def;
        minus = minus <= 0 ? 1 : minus;
        hp -= minus;
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

    public Sprite GetIcon() {
        return icon;
    }
}
