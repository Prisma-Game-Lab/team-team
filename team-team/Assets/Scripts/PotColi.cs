using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotColi : MonoBehaviour
{
    //J: variaveis que armazenam o tipo da poção, quem a arremessou (-1 = ninguem), e se está sendo segurada, para impedir que a poção estoure na mão de quem carrega
    public int potionType;
    private int thrower = -1;
    private bool thrown = false;

    void Start()
    {
        
    }
    
    void Update()
    {
        
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
    public void setThrown(bool newThrown)
    {
        thrown = newThrown;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Target"))
        {
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
