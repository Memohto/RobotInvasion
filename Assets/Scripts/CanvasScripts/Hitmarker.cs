using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hitmarker : MonoBehaviour
{
    public static Hitmarker Instance { private set; get; }
    [SerializeField]
    private RawImage[] images;

    private void Start() {
        Instance = this;
    }

    private IEnumerator ActivateHitmarker() {
        SetImageEnable(true);
        yield return new WaitForSeconds(0.1f);
        SetImageEnable(false);
    }

    private void SetImageEnable(bool val) {
        foreach (RawImage img in images) {
            img.enabled = val;
        }
    }
}
