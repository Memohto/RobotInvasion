using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour {

    [SerializeField]
    private Rigidbody rigidBody;
    [SerializeField]
    private float xForce;
    [SerializeField]
    private float yForce;
    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private AudioSource audioSrc;

    void Start() {
        rigidBody.AddForce(transform.forward*xForce + transform.up*yForce);
        SoundManagerScript.PlaySound("ball");
    }

    private void OnTriggerEnter(Collider other) {
        Collider[] hitted = Physics.OverlapSphere(transform.position,
                                                  10,
                                                  Boss.Instance.playerMask);
        foreach (Collider golpeado in hitted) {
            Player tmp = golpeado.gameObject.GetComponent<Player>();
            if (tmp != null) { 
                tmp.takeDmg(5);
            }
        }
        if (other.tag == "Player") {
            Player.Instance.takeDmg(50);
        }
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        audioSrc.Play();

        Destroy(gameObject,1);
        SoundManagerScript.PlaySound("explosion");
    }
}
