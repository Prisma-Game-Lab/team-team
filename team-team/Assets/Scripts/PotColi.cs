using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotColi : MonoBehaviour
{
    //J: variaveis que armazenam o tipo da poção, quem a arremessou (-1 = ninguem), e se está sendo segurada, para impedir que a poção estoure na mão de quem carrega
    //K: mudei para um enum para coexistir com meu outro script
    [Header("Variaveis definindo características básicas desta poção")]
    [Tooltip("O tipo de efeito que esta poção tem")]
    public PotionEffect potionType;
    [Tooltip("O valor em pontos que esta poção tem, caso acerte o calderão, caso a condição de vitória sejam pontos!")]
    public int pointValueOnCauldron = 100;
    [Tooltip("O valor em pontos que esta poção tem, caso acerte outro player, caso a condição de vitória sejam pontos!")]
    public int pointValueOnPlayer = 100;

    private int thrower = -1;
    private bool thrown = false;

    //J: variavel para definir distancia atravessada pela orbe antes de cair, e modo de queda (1 = linear, 2 = fisico)
    [Header("Variaveis para definir alcance e forma de arremeço da orbe. Ver tooltips para mais infos")]
    [Tooltip("Variavel para ajustar alcance do arremesso da orbe para lançamento linear")]
    public float throwRange;
    [Tooltip("Variavel que define forma que a orbe cai. (1 = em linha, 2 = em arco) ")]
    //K: isso tem toda a cara do mundo de um enum! Vale a pena trocar?
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
    //K: ótima prática! Encapsulamento é bom. Em C# tem uma jeito um pouco mais conciso de fazer a mesma coisa: pesquisa sobre Properties depois
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

    public void HitPlayer(PlayerEffects player)
    {
        GameController.potionCount--;
        player.AddEffect(potionType, 5.0f);

        //incrementa pontuação:
        GameController.Instance.AddPoints(pointValueOnPlayer, this.getThrower());
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Target"))
        {
            //J: destroi a poção ao colidir com o caldeirão
            GameController.potionCount--;
            Destroy(gameObject);
        }   
        // else if(thrown && !other.gameObject.CompareTag("Potion"))
        // {
        //     //J: PLACEHOLDER. aqui aplicará efeito da poção quando for implementado.
        //     Destroy(gameObject);
        //     //J: atualiza contador de poções, para criar nova poção
        //     GameController.potionCount--;
        // }
        else if(thrown && other.CompareTag("Player"))
        {
            //aplica efeito da poção!

            //J: atualiza contador de poções, para criar nova poção
            PlayerEffects pe = other.GetComponent<PlayerEffects>();
            Debug.Assert(pe != null);
            this.HitPlayer(pe);
            

        }
        
    }
}
