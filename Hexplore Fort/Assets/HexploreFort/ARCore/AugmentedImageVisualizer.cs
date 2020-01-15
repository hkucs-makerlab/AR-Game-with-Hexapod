using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class AugmentedImageVisualizer : MonoBehaviour
{
    public GameObject terrain;
    private AugmentedImage image;

    public void Initialize(AugmentedImage image) {
        this.image = image;
        float halfWidth = image.ExtentX / 2;
        float halfHeight = image.ExtentZ / 2;
        terrain.transform.localPosition = (halfWidth * Vector3.left) + (halfHeight * Vector3.back);
        terrain.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }
}
