using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HF_Static;

public class Door : MonoBehaviour {
    public StaticData.DOOR_TYPE type;

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Player")
        {
            if (GameManager.Instance.player.OpenDoor(type))
            {
                GetComponent<Animation>().Play();
                Invoke("DestroyDoor", 3f);
            }
        }
    }

    private void DestroyDoor() {
        Destroy(gameObject);
    }
}
