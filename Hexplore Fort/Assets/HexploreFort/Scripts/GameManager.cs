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

        Application.targetFrameRate = 60;
    }

    [SerializeField]
    public MovementJoystick movementJoystick;
    [SerializeField]
    public FightingJoystick fightingJoystick;

    [SerializeField]
    private GameObject startMenu, arGame;
    [SerializeField]
    private Canvas connectionCanvas;

    public Player player = new Player();
    public Text lv, hp, atk, def, money, exp, yellowKey, blueKey, redKey;

    private void Start() {
        //startMenu.SetActive(true);
        //connectionCanvas.enabled = false;
        //arGame.SetActive(false);
    }

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

    public void SwitchFighting(bool fight) {
        movementJoystick.gameObject.transform.parent.gameObject.SetActive(!fight);
        fightingJoystick.gameObject.transform.parent.gameObject.SetActive(fight);
    }


    public void StartGame() {
        connectionCanvas.enabled = false;
        startMenu.SetActive(false);
        arGame.SetActive(true);
    }

    public void Exit() {
        Application.Quit();
    }
}
