using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon {

    public static int maxAmmoCharger = 10;
    public static int maxAmmoTotal = 60;
    public static int damage = 2;

    [SerializeField]
    GameObject bala,source;

    protected new IEnumerator Shoot() {
        if (GameManager.Instance.AmmoCharger != 0) {
            CheckCollisions();
            GameObject balaTmp = Instantiate(bala, source.transform.position, mainCamera.transform.rotation);
            Destroy(balaTmp, 0.2f);
            GameManager.Instance.AmmoCharger = -1;
        } else {
            if (State != WeaponStates.Reload && GameManager.Instance.AmmoTotal > 0) {
                ChangeState(WeaponStates.Reload);
                StartCoroutine("Reload");
                
            }
        }
        yield return new WaitForEndOfFrame();
    }

    protected new IEnumerator Reload() {
        SoundManagerScript.PlaySound("reload");
        yield return new WaitForSeconds(1f);
        int ammoNeeded = maxAmmoCharger - GameManager.Instance.AmmoCharger;
        if (ammoNeeded > GameManager.Instance.AmmoTotal) {
            
            GameManager.Instance.AmmoCharger = GameManager.Instance.AmmoTotal;
            GameManager.Instance.AmmoTotal = -GameManager.Instance.AmmoTotal;
        } else {
            GameManager.Instance.AmmoCharger = ammoNeeded;
            GameManager.Instance.AmmoTotal = -ammoNeeded;
        }
        ChangeState(WeaponStates.Idle);
    }

    protected new void CheckCollisions() {
        SoundManagerScript.PlaySound("disparo");
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, 100f)) {
            if (hit.collider.tag == "Enemy") {
                GameManager.Instance.Hit();
                hit.collider.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            } else if (hit.collider.tag == "Boss") {
                GameManager.Instance.Hit();
                Boss.Instance.TakeDamage(damage);
            }
        }
    }
}
