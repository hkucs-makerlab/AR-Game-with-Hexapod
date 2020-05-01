using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HF_Static;

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
    private Button continueButton;
    private bool newGame;
    [SerializeField]
    private Canvas connectionCanvas;

    [SerializeField]
    private GameObject gameCanvas, keys, popupCanvas, storyCanvas;
    [SerializeField]
    public GameObject warningWindow, shoppingWindow, winningWindow;

    public Player player;
    public StaticData.GAME_PROGRESS progress;

    private void Start() {
        storyCanvas.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => ChangeProgress(StaticData.GAME_PROGRESS.MAP_GENERATION));

        //SaveSystem.DeleteAllData();
        player = SaveSystem.LoadPlayerInfo();
        continueButton.interactable = true;
        if (player == null) {
            player = new Player();
            continueButton.interactable = false;
        }

        ChangeProgress(StaticData.GAME_PROGRESS.START_MENU);
    }

    public void ChangeProgress(StaticData.GAME_PROGRESS toProgress) {
        switch (toProgress) {
            case StaticData.GAME_PROGRESS.START_MENU:
                startMenu.SetActive(true);
                storyCanvas.GetComponent<Canvas>().enabled = false;
                arGame.SetActive(false);
                RobotController.Instance.StopMovement();
                AudioManager.Instance.PlaySoundEffect(null);
                break;
            case StaticData.GAME_PROGRESS.STORY:
                startMenu.SetActive(true);
                arGame.SetActive(false);
                storyCanvas.GetComponent<Canvas>().enabled = true;
                storyCanvas.GetComponentInChildren<Animation>().Play();
                break;
            case StaticData.GAME_PROGRESS.MAP_GENERATION:
                HF_ARCoreController.Instance.ResetTracking();
                arGame.SetActive(true);
                HF_ARCoreController.Instance.SwitchToPlaneDetection();
                startMenu.SetActive(false);
                warningWindow.SetActive(false);
                winningWindow.SetActive(false);
                shoppingWindow.SetActive(false);
                gameCanvas.SetActive(false);
                movementJoystick.gameObject.transform.parent.gameObject.SetActive(false);
                fightingJoystick.gameObject.transform.parent.gameObject.SetActive(false);
                break;
            case StaticData.GAME_PROGRESS.ROBOT_RECOGNITION:
                arGame.SetActive(true);
                HF_ARCoreController.Instance.SwitchToImageRecognition();
                startMenu.SetActive(false);
                gameCanvas.SetActive(false);
                movementJoystick.gameObject.transform.parent.gameObject.SetActive(false);
                fightingJoystick.gameObject.transform.parent.gameObject.SetActive(false);
                break;
            case StaticData.GAME_PROGRESS.MOVING:
                arGame.SetActive(true);
                gameCanvas.SetActive(true);
                HF_ARCoreController.Instance.SwitchToGamePlay();
                movementJoystick.gameObject.transform.parent.gameObject.SetActive(true);
                fightingJoystick.gameObject.transform.parent.gameObject.SetActive(false);
                startMenu.SetActive(false);
                break;
            case StaticData.GAME_PROGRESS.FIGHTING:
                arGame.SetActive(true);
                gameCanvas.SetActive(true);
                fightingJoystick.gameObject.transform.parent.gameObject.SetActive(true);
                movementJoystick.gameObject.transform.parent.gameObject.SetActive(false);
                HF_ARCoreController.Instance.SwitchToGamePlay();
                startMenu.SetActive(false);
                break;
            case StaticData.GAME_PROGRESS.WINNING:
                winningWindow.SetActive(true);
                HF_ARCoreController.Instance.EndGame();
                SaveSystem.DeleteAllData();
                continueButton.interactable = false;
                fightingJoystick.gameObject.transform.parent.gameObject.SetActive(false);
                movementJoystick.gameObject.transform.parent.gameObject.SetActive(false);
                break;
        }
        progress = toProgress;
    }

    public void NewGame(bool newGame) {
        this.newGame = newGame;
    }

    public void StartGame() {
        connectionCanvas.enabled = false;
        continueButton.interactable = true;
        if (newGame) {
            SaveSystem.DeleteAllData();
            player = new Player();
            SaveSystem.SavePlayer(player);
            newGame = false;
            ChangeProgress(StaticData.GAME_PROGRESS.STORY);
            return;
        }

        if (progress == StaticData.GAME_PROGRESS.START_MENU) {
            ChangeProgress(StaticData.GAME_PROGRESS.MAP_GENERATION);
        } else {
            CloseSetting();
        }
    }

    public void OpenSetting() {
        Time.timeScale = 0;
        RobotController.Instance.StopMovement();
    }

    public void CloseSetting(bool continueMove = true) {
        Time.timeScale = 1;
        if (continueMove) {
            RobotController.Instance.ContinuePreviousMovement();
        }
    }

    public void ShowKey() {
        keys.SetActive(!keys.activeSelf);
    }

    public void PopupMessage(string message) {
        popupCanvas.transform.GetChild(1).gameObject.GetComponent<Text>().text = message;
        popupCanvas.GetComponent<Animation>().Play();
    }

    public void Exit() {
        Application.Quit();
    }
}
