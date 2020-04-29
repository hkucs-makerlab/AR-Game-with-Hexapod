using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HF_Static;

public class Shop : MonoBehaviour
{
    [SerializeField]
    private GameObject assistancePrefab, container;

    void Start()
    {
        foreach (Tuple<string, string, int, int> item in StaticData.SHOPPING_ITEMS) {
            GameObject assistance = Instantiate(assistancePrefab, container.transform);
            assistance.transform.GetChild(0).GetComponent<Text>().text = item.Item1 + " +" + item.Item3;
            assistance.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = item.Item4 + "";
            assistance.GetComponent<Button>().onClick.AddListener(() => Buy(item.Item1, item.Item2, item.Item3, item.Item4));
        }
    }

    private void Buy(string displayName, string attribute, int value, int price) {
        if (GameManager.Instance.player.Shop(attribute, value, price)) {
            GameManager.Instance.PopupMessage(displayName + " +" + value);
        } else {
            GameManager.Instance.PopupMessage("Not enough money!");
        }
    }
}
