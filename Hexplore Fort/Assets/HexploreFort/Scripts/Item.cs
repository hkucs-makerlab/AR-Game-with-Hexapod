using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HF_Static;

public class Item : MonoBehaviour
{
    public StaticData.ITEM_TYPE type;

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "Player") {
            if (GameManager.Instance.movementJoystick.pickingUp) {
                GameManager.Instance.player.PickUp(type);
                InitializeMap.Instance.map.PickupItem(transform.GetSiblingIndex());
                Destroy(gameObject);
            }
        }
    }
}
