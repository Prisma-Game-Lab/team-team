using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotColi : MonoBehaviour
{
    //J: variaveis que armazenam o tipo da poção, quem a arremessou (-1 = ninguem), e se está sendo segurada, para impedir que a poção estoure na mão de quem carrega
    public int potionType;
    private int thrower = -1;
    private bool thrown = false;

    //J: variavel para definir distancia atravessada pela orbe antes de cair, e modo de queda (1 = linear, 2 = fisico)
    [Header("Variaveis para definir alcance e forma de arremeço da orbe. Ver tooltips para mais infos")]
    [Tooltip("Variavel para ajustar alcance do arremesso da orbe para lançamento linear")]
    public float throwRange;
    [Tooltip("Variavel que define forma que a orbe cai. (1 = em linha, 2 = em arco) ")]
    public int fallType;

    //J: contador de tempo até fazer poção cair, para queda linear
    private float fallCounter;

    void Start()
    {

        Debug.Assert(fallType == 1 || fallType == 2);
        
        if (fallType == 1)
        {
            //J: tempo no ar são 60 frames * velocidade de arremesso do jogador/alcance de arremesso
            fallCounter = 60 * throwRange / GameController.getThrowSpeedGlobal() ;
            Debug.Log("fallCounter = " + fallCounter);
        }

    }
    
    void Update()
    {
        if (thrown )
        {
            //se queda linear, liga gravidade quando tempo acabar
            if (fallType == 1)
            {
                fallCounter--;
                if (fallCounter <= 0)
                {
                    this.GetComponent<Rigidbody>().useGravity = true;
                }
            }
            else if (fallType == 2)
            {

            }
        }

    }

    //J: get e set para a variavel thrower e set para a variavel thrown para serem utilizados no script Throw
    public int getThrower()
    {
        return thrower;
    }
    public void setThrower(int newThrower)
    {
        thrower = newThrower;
    }
    public bool getThrown()
    {
        return thrown;
    }
    public void setThrown(bool newThrown)
    {
        thrown = newThrown;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Target"))
        {
            //J: destroi a poção ao colidir com o caldeirão
            Destroy(gameObject);
        }   
        if(thrown && !other.gameObject.CompareTag("Potion"))
        {
            //J: PLACEHOLDER. aqui aplicará efeito da poção quando for implementado.
            Destroy(gameObject);
            //J: atualiza contador de poções, para criar nova poção
            GameController.potionCount--;
        }
        
    }
}
