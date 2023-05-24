using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour {

    public static Store Instance { private set; get; }

    [SerializeField]
    private GameObject gunPrefab;
    [SerializeField]
    private GameObject revolverPrefab;
    [SerializeField]
    private GameObject riflePrefab;
    [SerializeField]
    private GameObject portal;
    [SerializeField]
    private GameObject portalButton;
    [SerializeField]
    private Text gunBuyButton;
    [SerializeField]
    private Text revolverBuyButton;
    [SerializeField]
    private Text rifleBuyButton;
    [SerializeField]
    private Text cashText;
    
    private string owned = "";

    public float Cash {
        set {
            cash += value;
            cashText.text = "Cash: $" + cash;
        }
    }

    private float  cash = 0;

    void Start() {
        Instance = this;
    }

    public void BuyGun() {
        if (Stats.Instance.PlayerMoney >= 200 && !owned.Equals("gun") && owned.Equals("")) {
            Weapon gun = Instantiate(gunPrefab, WeaponContainer.Instance.transform).GetComponent<Weapon>();
            WeaponContainer.Instance.CurrentWeapon = gun;
            Player.Instance.Weapon = gun;
            Crosshair.Instance.Weapon = gun;
            Stats.Instance.PlayerMoney = -200;
            owned = "gun";
            gunBuyButton.text = "Owned";
            Ammo.Instance.AmmoCharger = Gun.maxAmmoCharger;
            Ammo.Instance.AmmoTotal = Gun.maxAmmoTotal;
            SoundManagerScript.PlaySound("register");
        } 
    }

    public void BuyRevolver() {
        if (Stats.Instance.PlayerMoney >= 500 && !owned.Equals("revolver") && owned.Equals("")) {
            Weapon revolver = Instantiate(revolverPrefab, WeaponContainer.Instance.transform).GetComponent<Weapon>();
            WeaponContainer.Instance.CurrentWeapon = revolver;
            Player.Instance.Weapon = revolver;
            Crosshair.Instance.Weapon = revolver;
            Stats.Instance.PlayerMoney = -500;
            owned = "revolver";
            revolverBuyButton.text = "Owned";
            Ammo.Instance.AmmoCharger = Revolver.maxAmmoCharger;
            Ammo.Instance.AmmoTotal = Revolver.maxAmmoTotal;
            SoundManagerScript.PlaySound("register");
        }
    }

    public void BuyRifle() {
        if (Stats.Instance.PlayerMoney >= 1000 && !owned.Equals("rifle") && owned.Equals("")) {
            Weapon rifle = Instantiate(riflePrefab, WeaponContainer.Instance.transform).GetComponent<Weapon>();
            WeaponContainer.Instance.CurrentWeapon = rifle;
            Player.Instance.Weapon = rifle;
            Crosshair.Instance.Weapon = rifle;
            Stats.Instance.PlayerMoney = -1000;
            owned = "rifle";
            rifleBuyButton.text = "Owned";
            Ammo.Instance.AmmoCharger = Rifle.maxAmmoCharger;
            Ammo.Instance.AmmoTotal = Rifle.maxAmmoTotal;
            SoundManagerScript.PlaySound("register");
        }
    }

    //TODO: Si vuelves a comprar la misma arma no se te reinicia la ammo
    public void Sell() {
        if (owned.Equals("gun")) {
            WeaponContainer.Instance.CurrentWeapon.Eliminate();
            WeaponContainer.Instance.CurrentWeapon = null;
            Player.Instance.Weapon = null;
            Crosshair.Instance.Weapon = null;
            Stats.Instance.PlayerMoney = 200;
            gunBuyButton.text = "Buy: $200";
            owned = "";
            Ammo.Instance.Reset();
            SoundManagerScript.PlaySound("register");
        } else if (owned.Equals("revolver")) {
            WeaponContainer.Instance.CurrentWeapon.Eliminate();
            WeaponContainer.Instance.CurrentWeapon = null;
            Player.Instance.Weapon = null;
            Crosshair.Instance.Weapon = null;
            Stats.Instance.PlayerMoney = 500;
            revolverBuyButton.text = "Buy: $500";
            owned = "";
            Ammo.Instance.Reset();
            SoundManagerScript.PlaySound("register");
        } else if (owned.Equals("rifle")) {
            WeaponContainer.Instance.CurrentWeapon.Eliminate();
            WeaponContainer.Instance.CurrentWeapon = null;
            Player.Instance.Weapon = null;
            Crosshair.Instance.Weapon = null;
            Stats.Instance.PlayerMoney = 1000;
            rifleBuyButton.text = "Buy: $1000";
            owned = "";
            Ammo.Instance.Reset();
            SoundManagerScript.PlaySound("register");
        }
    }

    public void BuyAmmo() {
        if (!owned.Equals("") && Stats.Instance.PlayerMoney >= 100) {
            if (owned.Equals("gun")) {
                Ammo.Instance.AmmoTotal = Gun.maxAmmoCharger;
                SoundManagerScript.PlaySound("register");
            } else if (owned.Equals("revolver")) {
                Ammo.Instance.AmmoTotal = Revolver.maxAmmoCharger;
                SoundManagerScript.PlaySound("register");
            } else if (owned.Equals("rifle")) {
                Ammo.Instance.AmmoTotal = Rifle.maxAmmoCharger;
                SoundManagerScript.PlaySound("register");
            }
            Stats.Instance.PlayerMoney = -100;
        }
    }

    public void UnlockPortal() {
        if (Stats.Instance.PlayerMoney >= 2000) {
            portal.SetActive(true);
            portalButton.SetActive(false);
            SoundManagerScript.PlaySound("register");
            Stats.Instance.PlayerMoney = -2000;
        }
    }
}
