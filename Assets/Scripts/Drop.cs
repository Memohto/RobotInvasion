using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rb;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }
    void Start()
    {
        Vector3 force = new Vector3(transform.forward.x*50f, 300f, transform.forward.z*50f);
        rb.AddForce(force);
        rb.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0,360f), 0f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            SoundManagerScript.PlaySound("item");
        }
    }
}
