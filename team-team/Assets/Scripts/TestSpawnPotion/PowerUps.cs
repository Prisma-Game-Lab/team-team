using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    //Arthur: Variável que define a duração dos efeitos dos power ups
    public float duration = 5.0f;

    //Arthur: Aplica o efeito das poções quando elas são coletadas (ainda vou mudar pra funcionar com colisão com o player)
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
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

        yield return new WaitForSeconds(duration);

        info.moveSpeed = minSpeed;
   
    }

    //Arthur: Poção que diminui a velocidade do player pela metade
    IEnumerator SpeedDown(Collider other)
    {
        float normalSpeed = 5.0f;
        float minSpeed = 2.5f;
        Move info = other.GetComponent<Move>();
        info.moveSpeed = minSpeed;

        yield return new WaitForSeconds(duration);

        info.moveSpeed = normalSpeed;

    }
}
