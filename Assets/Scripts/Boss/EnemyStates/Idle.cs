using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : MonoBehaviour {
    private Rigidbody rigidBody;
    private Vector3 startVector;
    private Vector3 randomVector;

    void Start() {
        rigidBody = GetComponent<Rigidbody>();
        startVector = new Vector3(146.6f, 6, -31);
        randomVector = startVector + new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20));
        Boss.Instance.animator.SetBool("IsRunning", true);
    }

    void Update() {
        if (Boss.Instance.life <= 0) {
            Boss.Instance.animator.SetBool("IsRunning", false);
            Boss.Instance.StateDone(true);
        }
        transform.LookAt(randomVector);
        transform.localPosition = Vector3.Lerp(transform.localPosition,
                                                  randomVector,
                                                  Time.deltaTime * 0.45f);
        if (EqualVectors(transform.position, randomVector, 2)) {
            Boss.Instance.animator.SetBool("IsRunning", false);
            Boss.Instance.StateDone();
        }  
    }

    private bool EqualVectors(Vector3 v1, Vector3 v2, float precision) {
        bool equal = true;

        if (Mathf.Abs(v1.x - v2.x) > precision) equal = false;
        if (Mathf.Abs(v1.y - v2.y) > precision) equal = false;
        if (Mathf.Abs(v1.z - v2.z) > precision) equal = false;

        return equal;
    }
}
