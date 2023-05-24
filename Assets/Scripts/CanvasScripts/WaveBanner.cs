using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveBanner : MonoBehaviour {
    public static WaveBanner Instance { private set; get; }

    [SerializeField]
    private RawImage waveBanner;
    [SerializeField]
    private RawImage restBackground;
    [SerializeField]
    private Text waveNumber;
    [SerializeField]
    private Text restTime;

    public int WaveNumber {
        set {
            waveNumber.text = "Wave " + value;
        }
    }

    private void Awake() {
        Instance = this;
        StartCoroutine(Test());
    }

    private IEnumerator Test() {
        yield return new WaitForSeconds(0);
        StartCoroutine("ActiveBackground");
        Stats.Instance.PlayerMoney = 200;
    }

    private IEnumerator ActiveBackground() {
        SpawnerManager.Instance.StopCoroutine("Spawning");
        Store.Instance.gameObject.SetActive(true);
        Player.Instance.InStore(true);
        GameManager.Instance.InGame(false);
        float cont = 20;
        //restTime.gameObject.SetActive(true);
        restBackground.gameObject.SetActive(true);
        while (cont >= 0) {
            restTime.text = cont + "";
            yield return new WaitForSeconds(1f);
            cont--;
        }
        Store.Instance.gameObject.SetActive(false);
        Player.Instance.InStore(false);
        GameManager.Instance.InGame(true);
        //restTime.gameObject.SetActive(false);
        restBackground.gameObject.SetActive(false);
        while (waveBanner.rectTransform.sizeDelta.x < Screen.width-100) {
            waveBanner.rectTransform.sizeDelta += new Vector2(50, 0);
            yield return new WaitForSeconds(0.03f);
        }
        cont = 0;
        waveNumber.gameObject.SetActive(true);
        while (cont < 3) {
            cont += 1;
            yield return new WaitForSeconds(1f);
        }

        waveNumber.gameObject.SetActive(false);
        while (waveBanner.rectTransform.sizeDelta.x > 0)
        {
            waveBanner.rectTransform.sizeDelta -= new Vector2(50, 0);
            yield return new WaitForSeconds(0.03f);
        }
        SpawnerManager.Instance.StartCoroutine("Spawning");
    }

    public void Desactivate() {
        restBackground.gameObject.SetActive(false);
    }
}
