using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBackgroundMusic : MonoBehaviour
{
    public static GameBackgroundMusic Instance { private set; get; }

    [SerializeField]
    private AudioSource audioSrc;

    private AudioClip game, boss;

    private void Start() {
        Instance = this;
        game = Resources.Load<AudioClip>("bgmusic");
        boss = Resources.Load<AudioClip>("BossTheme");
    }

    public void ChangeMusic() {
        audioSrc.clip = boss;
        audioSrc.Play();
    }
}
