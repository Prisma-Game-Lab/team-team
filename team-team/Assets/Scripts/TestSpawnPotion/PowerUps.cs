using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    //Arthur: Variável que define a duração dos efeitos dos power ups
    public float duration = 5.0f;

    //Arthur: Aplica o efeito das poções quando elas são coletadas (ainda vou mudar pra funcionar com colisão com o player)
        //J: ele aplicava o efeito da poção no player que pegasse porque aplica o efeito quando colide com player. Agora só aplica quando a orbe ja foi lançada
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && this.gameObject.GetComponent<PotColi>().getThrown())
        {
            if(this.gameObject.GetComponent<PotColi>().potionType == 0)
                StartCoroutine(SpeedUp(other));
            else if(this.gameObject.GetComponent<PotColi>().potionType == 1)
                StartCoroutine(SpeedDown(other));
        }
    }

    //Arthur: Poção que dobra a velocidade do player 
    IEnumerator SpeedUp(Collider other)
    {
        float maxSpeed = 10.0f;
        float minSpeed = 5.0f;
        Move info = other.GetComponent<Move>();
        info.moveSpeed = maxSpeed;

        Debug.Log("Acelerou");
        yield return new WaitForSeconds(duration);

        Debug.Log("Voltou a velocidade normal");

        info.moveSpeed = minSpeed;
   
    }

    //Arthur: Poção que diminui a velocidade do player pela metade
    IEnumerator SpeedDown(Collider other)
    {
        float normalSpeed = 5.0f;
        float minSpeed = 2.5f;
        Move info = other.GetComponent<Move>();
        info.moveSpeed = minSpeed;


        Debug.Log("Slow");
        yield return new WaitForSeconds(duration);
        Debug.Log("Voltou a velocidade normal");
        info.moveSpeed = normalSpeed;

    }
            //J: aparentemente pesquisei aqui e a duração das orbes nunca acaba porque o objeto que conta o tempo ta sendo destruido. temos que encontrar uma solução diferente depois
}
