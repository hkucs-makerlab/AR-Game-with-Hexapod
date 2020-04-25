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
    private Canvas connectionCanvas;

    [SerializeField]
    private GameObject planeController;
    [SerializeField]
    private GameObject[] planeVisualizer;
    [SerializeField]
    private GameObject imageController;
    [SerializeField]
    private GameObject[] imageVisualizer;

    [SerializeField]
    private GameObject gameCanvas, keys;

    public Player player = new Player();
    private StaticData.GAME_PROGRESS progress;

    private void Start() {
        ChangeProgress(StaticData.GAME_PROGRESS.START_MENU);
    }

    public void ChangeProgress(StaticData.GAME_PROGRESS toProgress) {
        switch (toProgress) {
            case StaticData.GAME_PROGRESS.START_MENU:
                startMenu.SetActive(true);
                arGame.SetActive(false);
                break;
            case StaticData.GAME_PROGRESS.MAP_GENERATION:
                arGame.SetActive(true);
                planeController.SetActive(true);
                foreach (GameObject visualizer in planeVisualizer) {
                    visualizer.SetActive(true);
                }
                startMenu.SetActive(false);
                imageController.SetActive(false);
                foreach (GameObject visualizer in imageVisualizer) {
                    visualizer.SetActive(false);
                }
                gameCanvas.SetActive(false);
                movementJoystick.gameObject.transform.parent.gameObject.SetActive(false);
                fightingJoystick.gameObject.transform.parent.gameObject.SetActive(false);
                break;
            case StaticData.GAME_PROGRESS.ROBOT_RECOGNITION:
                arGame.SetActive(true);
                imageController.SetActive(true);
                foreach (GameObject visualizer in imageVisualizer) {
                    visualizer.SetActive(true);
                }
                planeController.SetActive(true);
                foreach (GameObject visualizer in planeVisualizer) {
                    visualizer.SetActive(false);
                }
                startMenu.SetActive(false);
                gameCanvas.SetActive(false);
                movementJoystick.gameObject.transform.parent.gameObject.SetActive(false);
                fightingJoystick.gameObject.transform.parent.gameObject.SetActive(false);
                break;
            case StaticData.GAME_PROGRESS.MOVING:
                arGame.SetActive(true);
                gameCanvas.SetActive(true);
                movementJoystick.gameObject.transform.parent.gameObject.SetActive(true);
                fightingJoystick.gameObject.transform.parent.gameObject.SetActive(false);
                imageController.SetActive(true);
                foreach (GameObject visualizer in imageVisualizer) {
                    visualizer.SetActive(false);
                }
                planeController.SetActive(true);
                foreach (GameObject visualizer in planeVisualizer) {
                    visualizer.SetActive(false);
                }
                startMenu.SetActive(false);
                break;
            case StaticData.GAME_PROGRESS.FIGHTING:
                arGame.SetActive(true);
                gameCanvas.SetActive(true);
                fightingJoystick.gameObject.transform.parent.gameObject.SetActive(true);
                movementJoystick.gameObject.transform.parent.gameObject.SetActive(false);
                imageController.SetActive(true);
                foreach (GameObject visualizer in imageVisualizer) {
                    visualizer.SetActive(false);
                }
                planeController.SetActive(true);
                foreach (GameObject visualizer in planeVisualizer) {
                    visualizer.SetActive(false);
                }
                startMenu.SetActive(false);
                break;
        }
        progress = toProgress;
    }

    public void StartGame() {
        connectionCanvas.enabled = false;
        Time.timeScale = 1;
        if (progress == StaticData.GAME_PROGRESS.START_MENU) {
            ChangeProgress(StaticData.GAME_PROGRESS.MAP_GENERATION);
        }
    }

    public void OpenSetting() {
        Time.timeScale = 0;
    }

    public void CloseSetting() {
        Time.timeScale = 1;
    }

    public void ShowKey() {
        keys.SetActive(!keys.activeSelf);
    }

    public void Exit() {
        Application.Quit();
    }
}
