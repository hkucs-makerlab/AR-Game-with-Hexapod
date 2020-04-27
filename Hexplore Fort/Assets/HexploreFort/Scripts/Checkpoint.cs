using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HF_Static;

[System.Serializable]
public class Checkpoint : System.Object
{
    [SerializeField]
    public StaticData.CHECKPOINT type;
    [SerializeField]
    public int indexOfType, indexOfCheckpoint;

    public Checkpoint(StaticData.CHECKPOINT type, int indexOfType, int indexOfCheckpoint) {
        this.type = type;
        this.indexOfType = indexOfType;
        this.indexOfCheckpoint = indexOfCheckpoint;
    }
}
