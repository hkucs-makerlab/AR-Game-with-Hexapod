using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class HF_ARCoreController : MonoBehaviour {
    private static HF_ARCoreController _instance;
    public static HF_ARCoreController Instance { get { return _instance; } }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    [SerializeField]
    private GameObject device;

    [SerializeField]
    public GameObject planeController;
    [SerializeField]
    private GameObject[] planeVisualizer;

    [SerializeField]
    private GameObject imageController;
    [SerializeField]
    private GameObject[] imageVisualizer;

    public void SwitchToPlaneDetection() {
        planeController.SetActive(true);
        planeController.GetComponent<HF_ARCorePlaneController>().Retrack();
        foreach (GameObject visualizer in planeVisualizer) {
            visualizer.SetActive(true);
        }
        imageController.SetActive(false);
        foreach (GameObject visualizer in imageVisualizer) {
            visualizer.SetActive(false);
        }
    }

    public void SwitchToImageRecognition() {
        imageController.SetActive(true);
        imageController.GetComponent<HF_ARCoreImageController>().Retrack();
        foreach (GameObject visualizer in imageVisualizer) {
            visualizer.SetActive(true);
        }
        planeController.SetActive(true);
        foreach (GameObject visualizer in planeVisualizer) {
            visualizer.SetActive(false);
        }
    }

    public void SwitchToGamePlay() {
        imageController.SetActive(true);
        foreach (GameObject visualizer in imageVisualizer) {
            visualizer.SetActive(false);
        }
        planeController.SetActive(true);
        foreach (GameObject visualizer in planeVisualizer) {
            visualizer.SetActive(false);
        }
    }

    public void EndGame() {
        imageController.SetActive(false);
        foreach (GameObject visualizer in imageVisualizer) {
            visualizer.SetActive(false);
        }
        planeController.SetActive(false);
        foreach (GameObject visualizer in planeVisualizer) {
            visualizer.SetActive(false);
        }
    }

    public void ResetTracking() {
        ARCoreSession session = device.GetComponent<ARCoreSession>();
        ARCoreSessionConfig sessionConfig = session.SessionConfig;
        ARCoreCameraConfigFilter cameraConfigFilter = session.CameraConfigFilter;
        DestroyImmediate(session);
        session = device.AddComponent<ARCoreSession>();
        session.SessionConfig = sessionConfig;
        session.CameraConfigFilter = cameraConfigFilter;
        session.enabled = true;
    }
}
