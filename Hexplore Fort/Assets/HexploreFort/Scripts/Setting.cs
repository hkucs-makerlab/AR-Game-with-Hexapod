using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HF_Static;

public class Setting : MonoBehaviour
{
    [SerializeField]
    private Slider volumeSlider;

    private void Update() {
        volumeSlider.value = AudioManager.Instance.bgmSource.volume;
    }

    public void ChangeVolume(Slider volume) {
        AudioManager.Instance.bgmSource.volume = volume.value;
        AudioManager.Instance.effectSource.volume = volume.value;
    }

    public void ChangeProgress(int i) {
        GameManager.Instance.CloseSetting(false);
        StaticData.GAME_PROGRESS newProgress = (StaticData.GAME_PROGRESS)i;
        GameManager.Instance.ChangeProgress(newProgress);
    }
}
