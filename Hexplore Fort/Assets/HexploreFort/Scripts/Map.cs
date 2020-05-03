using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HF_Static;

[System.Serializable]
public class Map : System.Object
{
    private List<bool> doors, items, enemys;
    private List<Checkpoint> checkpoints;

    public Map(int numOfDoors, int numOfItems, int numOfEnemys, List<Checkpoint> checkpoints) {
        this.doors = new List<bool>();
        for (int i = 0; i < numOfDoors; i++) {
            this.doors.Add(true);
        }
        this.items = new List<bool>();
        for (int i = 0; i < numOfItems; i++) {
            this.items.Add(true);
        }
        this.enemys = new List<bool>();
        for (int i = 0; i < numOfEnemys; i++) {
            this.enemys.Add(true);
        }
        this.checkpoints = new List<Checkpoint>(checkpoints);
    }

    public Map(List<bool> doors, List<bool> items, List<bool> enemys, List<Checkpoint> checkpoints) {
        this.doors = new List<bool>(doors);
        this.items = new List<bool>(items);
        this.enemys = new List<bool>(enemys);
        this.checkpoints = new List<Checkpoint>(checkpoints);
    }

    public bool IsActiveDoor(int i) {
        return i < doors.Count ? doors[i] : false;
    }

    public bool IsActiveItem(int i) {
        return i < items.Count ? items[i] : false;
    }

    public bool IsActiveEnemy(int i) {
        return i < enemys.Count ? enemys[i] : false;
    }

    public bool IsActiveCheckpoint(int i) {
        bool active = true;
        bool contain = false;

        foreach (Checkpoint checkpoint in checkpoints) {
            if (checkpoint.indexOfCheckpoint == i) {
                contain = true;

                switch (checkpoint.type) {
                    case StaticData.CHECKPOINT.DOOR:
                        if (checkpoint.indexOfType < doors.Count) {
                            active = doors[checkpoint.indexOfType] ? active : false;
                        }
                        break;
                    case StaticData.CHECKPOINT.ENEMY:
                        if (checkpoint.indexOfType < enemys.Count) {
                            active = enemys[checkpoint.indexOfType] ? active : false;
                        }
                        break;
                }
            }
        }

        return contain ? active : false;
    }

    public int OpenDoor(int i) {
        doors[i] = false;
        Debug.Log(i);
        SaveSystem.SaveMap(this);

        foreach (Checkpoint checkpoint in checkpoints) {
            if (checkpoint.type == StaticData.CHECKPOINT.DOOR && checkpoint.indexOfType == i) {
                Debug.Log(checkpoint.indexOfCheckpoint);
                return checkpoint.indexOfCheckpoint;
            }
        }

        return -1;
    }

    public void PickupItem(int i) {
        items[i] = false;
        SaveSystem.SaveMap(this);
    }

    public int DefeatEnemy(int i) {
        Debug.Log(i);
        enemys[i] = false;
        SaveSystem.SaveMap(this);

        foreach (Checkpoint checkpoint in checkpoints) {
            if (checkpoint.type == StaticData.CHECKPOINT.ENEMY && checkpoint.indexOfType == i) {
                Debug.Log(checkpoint.indexOfCheckpoint);
                return checkpoint.indexOfCheckpoint;
            }
        }

        return -1;
    }
}
