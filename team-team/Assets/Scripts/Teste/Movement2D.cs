using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*

Classe teste que implementa uma versão simplíssima de movemento horizontal em 2D, nos eixos x e y.

obs: essa classe não implementa a ilusão de perspectiva!

Autores: Krauss
 */


public class Movement2D : MonoBehaviour
{
    public float speed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("up"))
        {
            transform.position += Vector3.up * speed * Time.deltaTime; 
        }
        if(Input.GetKey("down"))
        {
            transform.position += Vector3.down * speed * Time.deltaTime; 
        }
        if(Input.GetKey("left"))
        {
            transform.position += Vector3.left * speed * Time.deltaTime; 
        }
        if(Input.GetKey("right"))
        {
            transform.position += Vector3.right * speed * Time.deltaTime; 
        }
    }
}
