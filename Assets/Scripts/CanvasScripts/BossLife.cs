using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossLife : MonoBehaviour
{
    public static BossLife Instance { private set; get; }
    [SerializeField]
    private RawImage life;
    [SerializeField]
    private RawImage container;

    private void Start() {
        Instance = this;
    }

    void Update() {
        if (Boss.Instance != null) {
            life.rectTransform.sizeDelta = new Vector2(Boss.Instance.life*2, 25);
        }
    }

    public void Activate() {
        container.enabled = true;
        life.enabled = true;
    }
}
