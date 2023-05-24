using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{

    public static AudioClip jumpSound, hit, reloadSound, shotSound, runSound, deathSound, registerSound,itemSound, enemySound, bossSound, bossdeathSound, ballSound, snapSound, explosionSound;

    static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        jumpSound = Resources.Load<AudioClip>("jump");
        reloadSound = Resources.Load<AudioClip>("reload");
        shotSound = Resources.Load<AudioClip>("disparo");
        deathSound = Resources.Load<AudioClip>("death");
        runSound = Resources.Load<AudioClip>("correr");
        registerSound = Resources.Load<AudioClip>("register");
        itemSound = Resources.Load<AudioClip>("item");
        enemySound = Resources.Load<AudioClip>("enemy");
        bossdeathSound = Resources.Load<AudioClip>("bossdeath");
        bossSound = Resources.Load<AudioClip>("boss");
        ballSound = Resources.Load<AudioClip>("ball");
        snapSound = Resources.Load<AudioClip>("snap");
        explosionSound = Resources.Load<AudioClip>("explosion");
        hit = Resources.Load<AudioClip>("hit");

        audioSrc = GetComponent<AudioSource>();
    }


    public static void PlaySound (string clip)
    {
        switch (clip)
        {
            case "jump":
                audioSrc.PlayOneShot(jumpSound);
                break;
            case "reload":
                audioSrc.PlayOneShot(reloadSound);
                break;
            case "disparo":
                audioSrc.PlayOneShot(shotSound);
                break;
            case "register":
                audioSrc.PlayOneShot(registerSound);
                break;
            case "correr":
                audioSrc.PlayOneShot(runSound);
                break;
            case "death":
                audioSrc.PlayOneShot(deathSound);
                break;
            case "item":
                audioSrc.PlayOneShot(itemSound);
                break;
            case "enemy":
                audioSrc.PlayOneShot(itemSound);
                break;
            case "bossdeath":
                audioSrc.PlayOneShot(bossdeathSound);
                break;
            case "boss":
                audioSrc.PlayOneShot(bossSound);
                break;
            case "snap":
                audioSrc.PlayOneShot(snapSound);
                break;
            case "ball":
                audioSrc.PlayOneShot(ballSound);
                break;
            case "explosion":
                audioSrc.PlayOneShot(explosionSound);
                break;
            case "hit":
                audioSrc.PlayOneShot(hit);
                break;

        }
    }

}
