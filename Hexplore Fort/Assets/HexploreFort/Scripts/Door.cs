using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HF_Static;

public class Door : MonoBehaviour {
    public StaticData.DOOR_TYPE type;
    public GameObject checkpoint;

    public IEnumerator Open() {
        GetComponent<Animation>().Play();

        yield return new WaitForSeconds(3f);

        Destroy(gameObject);
    }
}
