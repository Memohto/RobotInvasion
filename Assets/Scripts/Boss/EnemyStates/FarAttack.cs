using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarAttack : MonoBehaviour {

    void Start() {
        StartCoroutine(Throw());
        StartCoroutine(Animate());
    }

    void Update() {
        if (Boss.Instance.life <= 0) {
            Boss.Instance.animator.SetBool("IsThrowing", false);
            Boss.Instance.StateDone(true);
        }
        transform.LookAt(Boss.Instance.player.transform.position);
    }

    private IEnumerator Throw() {
        yield return new WaitForSeconds(3f);
        Instantiate(Boss.Instance.bossBulletPrefab, Boss.Instance.bulletSpawner.position, Boss.Instance.transform.rotation);
        StartCoroutine(End());
    }


    private IEnumerator Animate() {
        yield return new WaitForSeconds(2.8f);
        Boss.Instance.animator.SetBool("IsThrowing", true);
    }

    private IEnumerator End() {
        Boss.Instance.animator.SetBool("IsThrowing", false);
        yield return new WaitForSeconds(1f);
        Boss.Instance.StateDone();
    }
}

