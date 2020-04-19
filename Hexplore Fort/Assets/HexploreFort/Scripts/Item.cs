using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HF_Static;

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
