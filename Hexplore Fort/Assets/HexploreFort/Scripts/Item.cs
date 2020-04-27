using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HF_Static;

public class Item : MonoBehaviour
{
    public StaticData.ITEM_TYPE type;
    public GameObject checkpoint;

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "Player") {
            if (GameManager.Instance.movementJoystick.pickingUp) {
                GameManager.Instance.player.PickUp(type);
                Destroy(gameObject);
            }
        }
    }
}
