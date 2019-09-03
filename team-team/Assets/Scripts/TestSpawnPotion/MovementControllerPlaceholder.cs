using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControllerPlaceholder : MonoBehaviour
{
    

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Input.GetAxis("Horizontal")*0.5f,0,Input.GetAxis("Vertical")*0.5f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Respawn"))
        {
            Destroy(other.gameObject);
            GameController.potionCount -= 1;
        }
    }
}
