using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    public Player player = new Player();
    public Text lv, hp, atk, def, money, exp, yellowKey, blueKey, redKey;

    private void Update()
    {
        lv.text = "" + player.GetLv();
        hp.text = "" + player.GetHp();
        atk.text = "" + player.GetAtk();
        def.text = "" + player.GetDef();
        money.text = "" + player.GetMoney();
        exp.text = "" + player.GetExp();
        yellowKey.text = "" + player.GetYellowKey();
        blueKey.text = "" + player.GetBlueKey();
        redKey.text = "" + player.GetRedKey();
    }
}
