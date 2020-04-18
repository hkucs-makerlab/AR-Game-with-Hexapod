using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class RobotController : MonoBehaviour {
    private static RobotController _instance;
    public static RobotController Instance { get { return _instance; } }

    public GameObject devicePrefab;
    public AndroidJavaObject controllerService;

    private Dictionary<string, string> devices;

    private void Awake() {
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
        if (controllerService != null) {
            bool scanning = controllerService.Call<bool>("startDiscovery");
            Debug.Log(scanning);
        }
    }

    public void OnScanResult(string result) {
        Debug.Log("received");
        Debug.Log(result);
        string[] device = result.Split(new[] { "%split%" }, StringSplitOptions.None);
        if (device[1] == "SPP-CA") {
            controllerService.Call("connectHexapod", device[0]);
            Debug.Log("Connect!!!");
        }
    }
}
