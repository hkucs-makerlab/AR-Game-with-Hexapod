using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HF_Static;

public class InitializeMap : MonoBehaviour {
    private static InitializeMap _instance;
    public static InitializeMap Instance { get { return _instance; } }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    public Map map;

    [SerializeField]
    private GameObject doors, items, enemys;
    public GameObject checkpoints;
    [SerializeField]
    private List<Checkpoint> checkpointsRelationship;

    private void Start() {
        map = SaveSystem.LoadMapInfo();
        if (map == null) {
            map = new Map(doors.transform.childCount, items.transform.childCount, enemys.transform.childCount, checkpointsRelationship);
        }

        SaveSystem.SaveMap(map);

        for (int i = 0; i < doors.transform.childCount; i++) {
            doors.transform.GetChild(i).gameObject.SetActive(map.IsActiveDoor(i));
        }

        for (int i = 0; i < items.transform.childCount; i++) {
            items.transform.GetChild(i).gameObject.SetActive(map.IsActiveItem(i));
        }

        for (int i = 0; i < enemys.transform.childCount; i++) {
            enemys.transform.GetChild(i).gameObject.SetActive(map.IsActiveEnemy(i));
        }

        for (int i = 0; i < checkpoints.transform.childCount; i++) {
            checkpoints.transform.GetChild(i).gameObject.SetActive(map.IsActiveCheckpoint(i));
        }
    }
}
