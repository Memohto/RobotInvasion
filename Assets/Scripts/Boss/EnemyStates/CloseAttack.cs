using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseAttack : MonoBehaviour {

    void Start() {
        StartCoroutine(Attack());
        StartCoroutine(Animate());
    }

    void Update() {
        if (Boss.Instance.life <= 0) {
            Boss.Instance.animator.SetBool("IsThrowing", false);
            Boss.Instance.StateDone(true);
        }
        transform.LookAt(Boss.Instance.player.transform.position);
    }

    private IEnumerator Attack() {
        yield return new WaitForSeconds(1f);
        Collider[] hitted = Physics.OverlapSphere(Boss.Instance.attackCenter.position, 
                                                  Boss.Instance.attackRange,
                                                  Boss.Instance.playerMask);
        foreach (Collider golpeado in hitted) {
            Player tmp = golpeado.gameObject.GetComponent<Player>();
            if (tmp != null) {
                tmp.takeDmg(50);
                tmp.BossHit(transform.forward, 700, 600);
            }
        }
        StartCoroutine(End());
        Boss.Instance.StateDone();
    }

    private IEnumerator Animate() {
        yield return new WaitForSeconds(0.8f);
        Boss.Instance.animator.SetBool("IsThrowing", true);
    }

    private IEnumerator End() {
        Boss.Instance.animator.SetBool("IsThrowing", false);
        yield return new WaitForSeconds(1f);
        Boss.Instance.StateDone();
    }
}
