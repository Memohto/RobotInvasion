using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { private set; get; }

    [Header("Menu Options")]
    [SerializeField]
    private Slider sensSlider;
    [SerializeField]
    private Text sensText;
    [Header("Wave Options")]
    [SerializeField]
    private int killsToEnd = 10;
    [SerializeField]
    private int roundIncrease = 2;

    private int totalKills = 0;
    private int waveKills = 0;
    private int wave = 0;

    private bool inGame = false;

    public float Sensibility { get; private set; }

    public int KillsToEnd
    {
        get { return Instance.killsToEnd; }
    }
    public float Stamina {
        set { Stats.Instance.PlayerStamina = value; }
        get { return Stats.Instance.PlayerStamina; }
    }

    public float Life {
        set { Stats.Instance.PlayerLife = value; }
        get { return Stats.Instance.PlayerLife; }
    }

    public float Money
    {
        set { Stats.Instance.PlayerMoney = value; }
        get { return Stats.Instance.PlayerMoney; }
    }

    public int AmmoCharger {
        set { Ammo.Instance.AmmoCharger = value; }
        get { return Ammo.Instance.AmmoCharger;  }
    }

    public int AmmoTotal {
        set { Ammo.Instance.AmmoTotal = value; }
        get { return Ammo.Instance.AmmoTotal; }
    }

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Sensibility = 3;
        sensSlider.value = Sensibility;
        DontDestroyOnLoad(gameObject);
    }

    private void Update() {
        /*if (Input.GetKeyDown(KeyCode.P)) {
            LoadScenes.Instance.EndGame();
        }*/
        Sensibility = sensSlider.value;
        sensText.text = "Sens: "+ Sensibility.ToString("F2");
        if (Player.Instance == null) return; 

        if (!Player.Instance.withBoss) {
            if (Input.GetKeyDown(KeyCode.B) && !inGame) {
                if (Store.Instance.gameObject.activeInHierarchy) {
                    Player.Instance.InStore(false);
                    Store.Instance.gameObject.SetActive(false);
                } else {
                    Store.Instance.gameObject.SetActive(true);
                    Player.Instance.InStore(true);
                }
            }
        } else {
            Player.Instance.InStore(false);
            WaveInfo.Instance.gameObject.SetActive(false);
            WaveBanner.Instance.Desactivate(); 
            WaveBanner.Instance.gameObject.SetActive(false);
            Store.Instance.gameObject.SetActive(false);
            BossLife.Instance.Activate();
        }
    }

    public void StartGame() {
        WaveBanner.Instance.StartCoroutine("ActiveBackground");
        Stats.Instance.PlayerMoney = 200;
    }

    #region Olas
    public void AddKill() {
        if (!Player.Instance.withBoss) {
            totalKills++;
            waveKills++;
            if (waveKills >= killsToEnd) {
                WaveInfo.Instance.WaveNumber = 1;
                WaveBanner.Instance.StartCoroutine("ActiveBackground");
                killsToEnd = Mathf.CeilToInt(killsToEnd * roundIncrease);
                waveKills = 0;
                SpawnerManager.Instance.EnemiesCreated = 0;
            }
            WaveInfo.Instance.WaveKills = waveKills;
            WaveInfo.Instance.TotalKills = totalKills;
        }
    }

    public void InGame(bool val) {
        inGame = val;
    }
    #endregion

    #region Hit
    public void Hit() {
        Hitmarker.Instance.StartCoroutine("ActivateHitmarker");
    }
    #endregion
}
