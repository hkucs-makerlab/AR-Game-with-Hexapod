using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HF_Static;

public class Door : MonoBehaviour {
    public StaticData.DOOR_TYPE type;
    public AudioClip clip;

    public IEnumerator Open() {
        int indexOfCheckpoint = InitializeMap.Instance.map.OpenDoor(transform.GetSiblingIndex());
        if (indexOfCheckpoint >= 0) {
            InitializeMap.Instance.checkpoints.transform.GetChild(indexOfCheckpoint).gameObject.SetActive(false);
        }
        GetComponent<Animation>().Play();
        AudioManager.Instance.PlaySoundEffect(clip);

        yield return new WaitForSeconds(3f);

        gameObject.SetActive(false);
    }
}
