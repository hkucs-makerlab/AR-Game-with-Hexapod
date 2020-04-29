using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using HF_Static;

public class HF_ARCoreImageController : MonoBehaviour {
    public HF_ARCoreVisualizer visualizerPrefab;
    public GameObject fitToScanOverlay;

    private Dictionary<int, HF_ARCoreVisualizer> visualizers = new Dictionary<int, HF_ARCoreVisualizer>();
    private List<AugmentedImage> tempAugmentedImages = new List<AugmentedImage>();

    public void Update() {
        _UpdateApplicationLifecycle();

        // Get updated augmented images for this frame.
        Session.GetTrackables<AugmentedImage>(tempAugmentedImages, TrackableQueryFilter.Updated);

        // Create visualizers and anchors for updated augmented images that are tracking and do
        // not previously have a visualizer. Remove visualizers for stopped images.
        foreach (var image in tempAugmentedImages) {
            HF_ARCoreVisualizer visualizer = null;
            visualizers.TryGetValue(image.DatabaseIndex, out visualizer);
            if (image.TrackingState == TrackingState.Tracking && visualizer == null) {
                // Create an anchor to ensure that ARCore keeps tracking this augmented image.
                Anchor anchor = image.CreateAnchor(image.CenterPose);
                visualizer = (HF_ARCoreVisualizer)Instantiate(visualizerPrefab, anchor.transform);
                visualizer.image = image;
                visualizer.map = HF_ARCoreController.Instance.planeController.GetComponent<HF_ARCorePlaneController>().visualizer;
                visualizers.Add(image.DatabaseIndex, visualizer);
                GameManager.Instance.ChangeProgress(StaticData.GAME_PROGRESS.MOVING);
            } else if (image.TrackingState == TrackingState.Stopped && visualizer != null) {
                visualizers.Remove(image.DatabaseIndex);
                GameObject.Destroy(visualizer.gameObject);
            }
        }

        // Show the fit-to-scan overlay if there are no images that are Tracking.
        foreach (var visualizer in visualizers.Values) {
            if (visualizer.image.TrackingState == TrackingState.Tracking) {
                fitToScanOverlay.SetActive(false);
                return;
            }
        }

        fitToScanOverlay.SetActive(true);
    }

    private void _UpdateApplicationLifecycle() {
        // Only allow the screen to sleep when not tracking.
        if (Session.Status != SessionStatus.Tracking) {
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
        } else {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
    }

    public void Retrack() {
        foreach (KeyValuePair<int, HF_ARCoreVisualizer> visualizer in visualizers) {
            if (visualizer.Value != null) {
                Destroy(visualizer.Value.gameObject.transform.parent.gameObject);
            }
        }
        visualizers.Clear();
    }
}
