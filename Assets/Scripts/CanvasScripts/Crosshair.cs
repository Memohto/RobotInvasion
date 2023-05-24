using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{    
    public static Crosshair Instance { private set; get; }
    [SerializeField]
    //0 -> N; 1 -> S; 2 -> E; 3 -> W
    private RawImage[] images = new RawImage[4];

    private Vector3[] startPos = new Vector3[4];

    public Weapon Weapon { set; get; }

    private void Start() {
        Instance = this;
        Weapon = null;
        for (int i = 0; i < startPos.Length; i++) {
            startPos[i] = images[i].rectTransform.position;
        }
    }

    void Update() {
        if (Weapon != null) {
            if (Weapon.State == WeaponStates.Aim) {
                if (images[0].enabled) {
                    foreach (RawImage img in images) {
                        img.enabled = false;
                    }
                }
            } else if (Weapon.State == WeaponStates.Idle) {
                for (int i = 0; i < images.Length; i++) {
                    images[i].rectTransform.position = Vector3.Lerp(images[i].rectTransform.position, startPos[i], Time.deltaTime * 10);
                }

                if (!images[0].enabled) {
                    foreach (RawImage img in images) {
                        img.enabled = true;
                    }
                }
            } else {
                float addedFloat;
                if (Weapon.State == WeaponStates.Run) {
                    addedFloat = 60f;
                } else {
                    addedFloat = 30f;
                }

                images[0].rectTransform.position = Vector3.Lerp(images[0].rectTransform.position, startPos[0] + new Vector3(0, addedFloat, 0), Time.deltaTime * 10);
                images[1].rectTransform.position = Vector3.Lerp(images[1].rectTransform.position, startPos[1] + new Vector3(0, -addedFloat, 0), Time.deltaTime * 10);
                images[2].rectTransform.position = Vector3.Lerp(images[2].rectTransform.position, startPos[2] + new Vector3(addedFloat, 0, 0), Time.deltaTime * 10);
                images[3].rectTransform.position = Vector3.Lerp(images[3].rectTransform.position, startPos[3] + new Vector3(-addedFloat, 0, 0), Time.deltaTime * 10);

                if (!images[0].enabled) {
                    foreach (RawImage img in images) {
                        img.enabled = true;
                    }
                }
            }
        }
    }
}
