using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ammo : MonoBehaviour {

    public static Ammo Instance { private set; get; }

    [SerializeField]
    private Text ammo;

    public int AmmoCharger {
        set {
            ammoCharger += value;
            ammo.text = ammoCharger + "\n" + ammoTotal;
        }
        get { return ammoCharger; }
    }

    public int AmmoTotal {
        set {
            ammoTotal += value;
            ammo.text = ammoCharger + "\n" + ammoTotal;
        }
        get { return ammoTotal; }
    }

    private int ammoCharger;
    private int ammoTotal;

    private void Start() {
        Instance = this;
        Reset();
    }

    public void Reset() {
        AmmoCharger = -AmmoCharger;
        AmmoTotal = -AmmoTotal;
    }
}
