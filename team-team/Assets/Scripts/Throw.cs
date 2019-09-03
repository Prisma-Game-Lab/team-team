using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    public Transform holdpoint;
    public bool holding = false;
    public Rigidbody rb;
    public float throwSpeed = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (holding)
        {
            rb.transform.position = holdpoint.position;
            rb.useGravity = false;
            if (Input.GetKeyDown("space"))
            { 
                rb.velocity = new Vector3(transform.localScale.x, 0.0f, 0.0f) * throwSpeed;
                rb.useGravity = true;
                holding = false;
            }
        }
            
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Potion") && holding == false)
        {
            holding = true;
            other.gameObject.transform.SetParent(holdpoint);
            other.gameObject.transform.position = holdpoint.position;
            other.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
