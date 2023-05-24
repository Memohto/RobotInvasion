using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveInfo : MonoBehaviour
{
    public static WaveInfo Instance { private set; get; }

    [SerializeField]
    private Text waveNumberTxt;
    [SerializeField]
    private Text waveKillsTxt;
    [SerializeField]
    private Text totalKillsTxt;

    public int WaveNumber {
        set {
            waveNumber += value;
            waveNumberTxt.text = "Wave: " + waveNumber;
            WaveBanner.Instance.WaveNumber = waveNumber;
        }
        get { return waveNumber;  }
    }

    public int WaveKills {
        set { waveKillsTxt.text = "Kills: " + value + "/" + GameManager.Instance.KillsToEnd; }
        get { return Instance.WaveKills; }
    }

    public int TotalKills {
        set { totalKillsTxt.text = "Total kills: " + value; }
        get { return Instance.TotalKills; }
    }

    private int waveNumber = 1;
    private int totalKills = 0;

    void Start() {
        Instance = this;
    }
}
