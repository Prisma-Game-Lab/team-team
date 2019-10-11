using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotColi : MonoBehaviour
{
    public enum FallStyle { linha, arco}
    //J: variaveis que armazenam o tipo da poção, quem a arremessou (-1 = ninguem), e se está sendo segurada, para impedir que a poção estoure na mão de quem carrega
    //K: mudei para um enum para coexistir com meu outro script
    [Header("Variaveis definindo características básicas desta poção")]
    [Tooltip("O tipo de efeito que esta poção tem")]
    public PotionEffect potionType;
    [Tooltip("O valor em pontos que esta poção tem, caso acerte o calderão, caso a condição de vitória sejam pontos!")]
    public int pointValueOnCauldron = 100;
    [Tooltip("O valor em pontos que esta poção tem, caso acerte outro player, caso a condição de vitória sejam pontos!")]
    public int pointValueOnPlayer = 100;

    [Header("Ajuste das durações dos efeitos")]
    [Tooltip("Duração do efeito da poção")]
    public float potionDuration;

    private int thrower = -1;
    private bool thrown = false;

    //J: variavel para definir distancia atravessada pela orbe antes de cair, e modo de queda (1 = linear, 2 = fisico)
    [Header("Variaveis para definir alcance e forma de arremeço da orbe. Ver tooltips para mais infos")]
    [Tooltip("Variavel que define forma que a orbe cai")]
    public FallStyle fallType;
    [Tooltip("Variavel que define a altura que a orbe alcança, quando em queda do tipo arco, ou o tempo antes de cair, para queda em linha")]
    public int airTime;

    void Start()
    {
    }
    
    void Update()
    {
        if (thrown )
        {

            airTime--;

            //J: se queda linear, liga gravidade quando tempo acabar
            if (fallType == FallStyle.linha)
            {
                if (airTime <= 0)
                {
                    this.GetComponent<Rigidbody>().useGravity = true;
                }
            }

            //J: se queda  em arco, faz cair.
            else if (fallType == FallStyle.arco)
            {
                    this.GetComponent<Rigidbody>().useGravity = true;
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

        //J: se tiver trajetoria de arco, cria velocidade vertical
        if (newThrown && fallType == FallStyle.arco)
        {
            gameObject.GetComponent<Rigidbody>().AddForce(0, airTime, 0, ForceMode.VelocityChange);
        }
    }

    public void HitPlayer(PlayerEffects player)
    {
        player.AddEffect(potionType, potionDuration);

        //incrementa pontuação:
        GameController.Instance.AddPoints(pointValueOnPlayer, this.getThrower());
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            //J: destroi a poção ao colidir com o caldeirão
            Destroy(gameObject);
            GameController.potionCount--;
        }   
        else if(thrown && other.CompareTag("Player"))
        {

            //J: atualiza contador de poções, para criar nova poção
            Destroy(gameObject);
            PlayerEffects pe = other.GetComponent<PlayerEffects>();
            Debug.Assert(pe != null);
            this.HitPlayer(pe);
            GameController.potionCount--;
        }
        else if (thrown)
        {
            Destroy(gameObject);
            GameController.potionCount--;
        }
        
    }
}
