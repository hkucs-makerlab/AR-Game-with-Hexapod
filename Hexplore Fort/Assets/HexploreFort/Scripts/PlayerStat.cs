using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour
{
    [SerializeField]
    private Text hp, money, lv, exp, atk, def, yellowKey, blueKey, redKey;

    private Player player;

    private void OnEnable() {
        player = GameManager.Instance.player;
    }

    void Update()
    {
        hp.text = player.GetHp() + "";
        money.text = player.GetMoney() + "";
        lv.text = "Lv. " + player.GetLv();
        exp.text = "EXP " + player.GetExp() + "/100";
        atk.text = player.GetAtk() + "";
        def.text = player.GetDef() + "";
        yellowKey.text = player.GetYellowKey() + "";
        blueKey.text = player.GetBlueKey() + "";
        redKey.text = player.GetRedKey() + "";
    }
}
