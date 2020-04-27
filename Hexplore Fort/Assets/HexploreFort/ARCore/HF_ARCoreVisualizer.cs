using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class HF_ARCoreVisualizer : MonoBehaviour {
    public AugmentedImage image;
    public GameObject visualizerObject;
    public GameObject map;
    private GameObject warningWindow;
    private bool warning;

    private void Start() {
        warningWindow = GameManager.Instance.warningWindow;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Checkpoint") {
            warningWindow.SetActive(true);
            visualizerObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Checkpoint") {
            warningWindow.SetActive(false);
            visualizerObject.GetComponent<BoxCollider>().enabled = true;
        }
    }

    private void Update() {
        if (image == null || image.TrackingState != TrackingState.Tracking || map == null) {
            visualizerObject.SetActive(false);
            return;
        }

        transform.position = new Vector3(transform.position.x, map.transform.position.y, transform.position.z);
        //transform.rotation = Quaternion.Euler(map.transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, map.transform.rotation.eulerAngles.z);
        visualizerObject.SetActive(true);
    }
}
