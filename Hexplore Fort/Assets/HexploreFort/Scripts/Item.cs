using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public StaticData.ITEM_TYPE type;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.Instance.player.PickUp(type);
            Destroy(gameObject);
        }
    }
}
