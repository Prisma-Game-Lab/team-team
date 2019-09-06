using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
Throw: script que implementa o comportamento de um player segurar objetos e poder arremessá-los

Autores: Arthur, Krauss

 */
public class Throw : MonoBehaviour
{    
    public Transform holdpoint;
    public bool holding = false;
    
    //Arthur, é de fato importante ter essa variável Rigidbody aqui. Mas é importante que o nome dela seja potionRigidbody, ou algo mais descritivo que só rb. Tomei a liberdade de mudar. Ass: Krauss
        //Além disso, é importante que ela seja setada quando o player pega uma poção, senão ele só poderia pegar uma única poção durante o jogo
    private Rigidbody potionRigidbody;
    public float throwSpeed = 100.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //se o personagem está segurando uma poção,
        if (holding)
        {
            Debug.Assert(potionRigidbody != null);

            //seta a posição da poção
            potionRigidbody.transform.position = holdpoint.position;
            potionRigidbody.useGravity = false;
            
            if (Input.GetKeyDown("space"))
            { 
                //arremessa a poção que estava sendo segurada
                
                //arthur, eu alterei esta linha abaixo para fazer com que o objeto seja arremessado em várias direções, e não só para direita! Ass: Krauss
                //old: potionRigidbody.velocity = new Vector3(transform.localScale.x, 0.0f, 0.0f) * throwSpeed;
                
                Vector3 throwDirection = transform.forward;
                Debug.Log(throwDirection);
                potionRigidbody.velocity =  throwDirection * throwSpeed;
                potionRigidbody.gameObject.transform.SetParent(null);
                //potionRigidbody.useGravity = true;
                holding = false;
            }
            
        }
            
    }


    // on TRIGGER enter
    private void OnTriggerEnter(Collider other)
    {
        //se o player colidir com a poção && o player não estiver segurando outra poção,
        if(other.gameObject.CompareTag("Potion") && holding == false)
        {
            //segura esta poção:
            holding = true;
            other.gameObject.transform.SetParent(holdpoint);
            other.gameObject.transform.position = holdpoint.position;
            potionRigidbody = other.gameObject.GetComponent<Rigidbody>();
            potionRigidbody.isKinematic = false;
        }
    }
}
