using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static Player player = new Player();
    public Text lv, hp, atk, def, money, exp, yellowKey, blueKey, redKey;

    void Start()
    {
        
    }

    private void Update()
    {
        lv.text = "LV: " + player.GetLv();
        hp.text = "HP: " + player.GetHp();
        atk.text = "ATK: " + player.GetAtk();
        def.text = "DEF: " + player.GetDef();
        money.text = "$ " + player.GetMoney();
        exp.text = "EXP: " + player.GetExp();
        yellowKey.text = "Yellow: " + player.GetYellowKey();
        blueKey.text = "Blue: " + player.GetBlueKey();
        redKey.text = "Red: " + player.GetRedKey();
    }
}
