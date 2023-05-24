using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    public static Stats Instance { get; set; }

    [SerializeField]
    private Text[] statsTexts;

    public float PlayerLife {
        set {
            playerLife = value;
            statsTexts[0].text = playerLife + "%";
        }
        get { return playerLife; }
    }

    public float PlayerStamina {
        set {
            playerStamina = value;
            statsTexts[1].text = playerStamina + "%";
        }
        get { return playerStamina; }
    }

    public float PlayerMoney
    {
        set
        {
            playerMoney += value;
            statsTexts[2].text = "$" + playerMoney;
            Store.Instance.Cash = value;
        }
        get { return playerMoney; }
    }

    private float playerLife;
    private float playerStamina;
    private float playerMoney;

    private void Awake() {
        Instance = this;
        playerLife = Player.INIT_LIFE;
        playerStamina = Player.INIT_STAMINA;
    }
}
