using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAttack : MonoBehaviour {

    void Start() {
        StartCoroutine(Spawn());
        StartCoroutine(Animate());
        SoundManagerScript.PlaySound("snap");

    }

    void Update() {
        if (Boss.Instance.life <= 0) {
            Boss.Instance.animator.SetBool("IsSpawning", false);
            Boss.Instance.StateDone(true);
        }
        transform.LookAt(Boss.Instance.player.transform.position);
    }

    private IEnumerator Spawn() {
        yield return new WaitForSeconds(3f);
        Instantiate(Boss.Instance.robotBrincadorPrefab, Boss.Instance.spawner1.position, Quaternion.identity);
        Instantiate(Boss.Instance.robotBrincadorPrefab, Boss.Instance.spawner2.position, Quaternion.identity);
        Instantiate(Boss.Instance.robotPrefab, Boss.Instance.spawner3.position, Quaternion.identity);
        Instantiate(Boss.Instance.robotPrefab, Boss.Instance.spawner4.position, Quaternion.identity);
        StartCoroutine(End());

    }

    private IEnumerator Animate() {
        yield return new WaitForSeconds(2f);
        Boss.Instance.animator.SetBool("IsSpawning", true);
    }

    private IEnumerator End() {
        Boss.Instance.animator.SetBool("IsSpawning", false);
        yield return new WaitForSeconds(1f);
        Boss.Instance.StateDone();
    }
}
