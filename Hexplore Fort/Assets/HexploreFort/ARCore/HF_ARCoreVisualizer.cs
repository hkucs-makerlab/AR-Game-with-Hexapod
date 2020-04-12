using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class HF_ARCoreVisualizer : MonoBehaviour {
    public AugmentedImage image;
    public GameObject terrain;

    public void Update() {
        if (image == null || image.TrackingState != TrackingState.Tracking) {
            terrain.SetActive(false);
            return;
        }

        /*float halfWidth = image.ExtentX / 2;
        float halfHeight = image.ExtentZ / 2;
        terrain.transform.localPosition =
            (halfWidth * Vector3.left) + (halfHeight * Vector3.back);
            */
        terrain.SetActive(true);
    }
}
