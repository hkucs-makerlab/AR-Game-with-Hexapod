using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HF_Static;

public class Setting : MonoBehaviour
{
    public void ChangeVolume(Slider volume) {
        Debug.Log(volume.value);
    }

    public void ReconnectHexapod() {
        Time.timeScale = 0;
    }

    public void ChangeProgress(int i) {
        gameObject.SetActive(false);
        StaticData.GAME_PROGRESS newProgress = (StaticData.GAME_PROGRESS)i;
        GameManager.Instance.ChangeProgress(newProgress);
    }
}
