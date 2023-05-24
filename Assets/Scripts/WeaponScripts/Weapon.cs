using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon: MonoBehaviour {

    public WeaponStates State { private set; get; }

    protected Camera mainCamera;

    private void Start() {
        mainCamera = Camera.main;
        State = WeaponStates.Idle;
    }

    private void Update() {
        if (Player.Instance.State == PlayerStates.Move || Player.Instance.State == PlayerStates.Run) {
            if (State != WeaponStates.Reload) {
                if (Input.GetMouseButton(1)) {
                    ChangeState(WeaponStates.Aim);
                } else {
                    if (Player.Instance.State == PlayerStates.Move) {
                        ChangeState(WeaponStates.Move);
                    } else if (Player.Instance.State == PlayerStates.Run) {
                        ChangeState(WeaponStates.Run);
                    }
                }
            }
        } else {
            if (State != WeaponStates.Reload) {
                if (Input.GetMouseButton(1)) {
                    ChangeState(WeaponStates.Aim);
                } else {
                    ChangeState(WeaponStates.Idle);
                }
            }
        }

        if (State != WeaponStates.Aim || State != WeaponStates.Reload) {
            if (!Player.Instance.IsGrounded()) {
                ChangeState(WeaponStates.Jump);
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && GameManager.Instance.AmmoTotal > 0) {
            StartCoroutine("Reload");
            ChangeState(WeaponStates.Reload);
        }
    }

    protected IEnumerator Shoot() {
        yield return new WaitForEndOfFrame();
    }

    protected IEnumerator Reload() {
        yield return new WaitForEndOfFrame();
    }

    protected void CheckCollisions() { }

    protected void ChangeState(WeaponStates newState) {
        State = newState;
        WeaponContainer.Instance.State = newState;
    }

    public void Eliminate() {
        Destroy(gameObject);
    }
}
