using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class HF_ARCoreVisualizer : MonoBehaviour {
    public AugmentedImage image;
    public GameObject visualizerObject;
    public GameObject map;

    public void Update() {
        if (image == null || image.TrackingState != TrackingState.Tracking || map == null) {
            visualizerObject.SetActive(false);
            return;
        }

        /*float halfWidth = image.ExtentX / 2;
        float halfHeight = image.ExtentZ / 2;
        terrain.transform.localPosition =
            (halfWidth * Vector3.left) + (halfHeight * Vector3.back);
            */

        transform.position = new Vector3(transform.position.x, map.transform.position.y, transform.position.z);
        //transform.rotation = Quaternion.Euler(map.transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, map.transform.rotation.eulerAngles.z);
        visualizerObject.SetActive(true);
    }
}
