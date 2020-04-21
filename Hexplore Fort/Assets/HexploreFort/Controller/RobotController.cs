﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using HF_Static;

public class RobotController : MonoBehaviour {
    private static RobotController _instance;
    public static RobotController Instance { get { return _instance; } }

    public GameObject devicePrefab, container;
    public Canvas blockerCanvas, connectionCanvas;
    public Button startButton;

    public AndroidJavaObject controllerService;

    private Dictionary<string, string> devices;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }

        devices = new Dictionary<string, string>();
    }

    private void Start() {
        if (!Permission.HasUserAuthorizedPermission(Permission.CoarseLocation)) {
            Permission.RequestUserPermission(Permission.CoarseLocation);
        }

        AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");

        AndroidJavaClass controllerServiceClass = new AndroidJavaClass("com.hkucsfyp.controller.ControllerService");
        if (controllerServiceClass != null) {
            controllerService = controllerServiceClass.CallStatic<AndroidJavaObject>("createInstance", activityContext);
        }
    }

    public void FindHexapod() {
        foreach (Transform child in container.transform) {
            Destroy(child.gameObject);
        }
        if (controllerService != null) {
            bool scanning = controllerService.Call<bool>("startDiscovery");
        }
    }

    public void OnScanResult(string result) {
        string[] deviceInfo = result.Split(new[] { "%split%" }, StringSplitOptions.None);
        GameObject newDevice = Instantiate(devicePrefab, container.transform);
        newDevice.transform.GetChild(0).gameObject.GetComponent<Text>().text = deviceInfo[0];
        if (deviceInfo[1] == "null") {
            newDevice.transform.GetChild(2).gameObject.GetComponent<Text>().text = "Unknown";
        } else {
            newDevice.transform.GetChild(2).gameObject.GetComponent<Text>().text = deviceInfo[1];
        }
        newDevice.GetComponent<Button>().onClick.AddListener(() => ConnectHexapod(deviceInfo[0]));
    }

    public void ConnectHexapod(string address) {
        if (controllerService != null) {
            controllerService.Call("connectHexapod", address);
        }
    }

    public void OnConnectionResult(string result) {
        if (result == "Connected") {
            startButton.interactable = true;
            blockerCanvas.enabled = false;
            connectionCanvas.enabled = false;
        }
    }

    public void SetMovementMode(StaticData.MODE_LETTER modeLetter, StaticData.MODE_NUMBER modeNumber, StaticData.DPAD_LETTER dPadLetter) {
        string movement = modeLetter.ToString() + (int)modeNumber + dPadLetter.ToString();
        if (controllerService != null) {
            controllerService.Call("setMovement", movement);
        }
    }
}