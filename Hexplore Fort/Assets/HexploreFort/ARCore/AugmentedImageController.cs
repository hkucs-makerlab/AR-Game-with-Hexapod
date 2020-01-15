﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class AugmentedImageController : MonoBehaviour
{
    public AugmentedImageVisualizer AugmentedImageVisualizerPrefab;
    public GameObject FitToScanOverlay;

    private Dictionary<int, AugmentedImageVisualizer> m_Visualizers
        = new Dictionary<int, AugmentedImageVisualizer>();

    private List<AugmentedImage> m_TempAugmentedImages = new List<AugmentedImage>();

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        FitToScanOverlay.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        Session.GetTrackables<AugmentedImage>(
                m_TempAugmentedImages, TrackableQueryFilter.Updated);

        // Create visualizers and anchors for updated augmented images that are tracking and do
        // not previously have a visualizer. Remove visualizers for stopped images.
        foreach (var image in m_TempAugmentedImages)
        {
            AugmentedImageVisualizer visualizer = null;
            m_Visualizers.TryGetValue(image.DatabaseIndex, out visualizer);
            if (image.TrackingState == TrackingState.Tracking && visualizer == null)
            {
                // Create an anchor to ensure that ARCore keeps tracking this augmented image.
                Anchor anchor = image.CreateAnchor(image.CenterPose);
                visualizer = (AugmentedImageVisualizer)Instantiate(AugmentedImageVisualizerPrefab, anchor.transform);
                visualizer.Initialize(image);
                m_Visualizers.Add(image.DatabaseIndex, visualizer);
                FitToScanOverlay.SetActive(false);
                this.enabled = false;
            }
            //else if (image.TrackingState == TrackingState.Stopped && visualizer != null)
            //{
            //    m_Visualizers.Remove(image.DatabaseIndex);
            //    GameObject.Destroy(visualizer.gameObject);
            //}
        }
    }
}
