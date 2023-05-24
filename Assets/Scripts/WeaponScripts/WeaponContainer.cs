using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponContainer : MonoBehaviour
{
    public static WeaponContainer Instance { private set; get; }

    [SerializeField]
    private Player player;

    private Vector3 startPos;
    private Quaternion startRot;
    private float moveCount = 0;

    public WeaponStates State { set; get; }
    public Weapon CurrentWeapon { set; get; }

    void Start() {
        Instance = this;
        CurrentWeapon = null;
        startPos = transform.localPosition;
        startRot = transform.rotation;
    }

    void Update() {
        WeaponAnimation();
    }
    
    public void WeaponAnimation() {
        switch (State) {
            case WeaponStates.Idle:
                moveCount += Time.deltaTime;
                Animate(new Vector3(0, 0.06f * Mathf.Sin(moveCount), 0), Vector3.zero, 10, 10);                    
                break;
            case WeaponStates.Move:
                moveCount += Time.deltaTime * 5;
                Animate(new Vector3(0.06f * Mathf.Cos(moveCount), 0.06f * Mathf.Sin(moveCount * 2), 0), Vector3.zero, 10, 10);
                break;
            case WeaponStates.Run:
                moveCount += Time.deltaTime * 10;
                Animate(new Vector3(0.06f * Mathf.Cos(moveCount), 0.06f * Mathf.Sin(moveCount * 2), 0), Vector3.zero, 10, 10);
                break;
            case WeaponStates.Aim:
                Animate(new Vector3(0.525f, -0.28f, 0.6f), new Vector3(-5, 0, 0), 10, 5, true);
                break;
            case WeaponStates.Jump:
                Animate(new Vector3(0, 0.15f, 0), new Vector3(5, 0, 0), 10, 5);
                break;
            case WeaponStates.Reload:
                Animate(new Vector3(0, -0.6f, 0), Vector3.zero, 10, 10);
                break;
        }
    }

    private void Animate(Vector3 newPos, Vector3 newRot, int posFactor, int rotFactor, bool aim = false) {
        if (!aim) {
            transform.localPosition = Vector3.Lerp(transform.localPosition,
                                                   startPos + newPos,
                                                   Time.deltaTime * posFactor);
        } else {
            transform.localPosition = Vector3.Lerp(transform.localPosition,
                                                   newPos,
                                                   Time.deltaTime * posFactor);
        }
        transform.localRotation = Quaternion.Lerp(transform.localRotation, 
                                                  Quaternion.Euler(startRot.eulerAngles + newPos), 
                                                  Time.deltaTime * rotFactor);
    }
}
