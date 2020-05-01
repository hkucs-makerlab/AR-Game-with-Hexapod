using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    [SerializeField]
    public AudioSource bgmSource, effectSource;

    public void PlaySoundEffect(AudioClip clip, bool loop = false) {
        effectSource.loop = loop;
        effectSource.clip = clip;
        effectSource.Play();
    }
}
