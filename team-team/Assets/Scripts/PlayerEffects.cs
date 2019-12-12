using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
PlayerEffects: 
Efeito para gerenciar os efeitos correntes para um determinado jogador. 
Para a maior parte das poções os efeitos vão simplesmente se acumular, 
mas quaisquer interalções especiais entre poções deve ser implementada aqui

Autores: Krauss, 
 */



public enum PotionEffect
{
    Nothing,
    Accelerate, 
    Decelerate,
    Freeze,
    Invert
}

public class PlayerEffects : MonoBehaviour
{
    // Start is called before the first frame update

    //um dicionario guardando todos os efeitos correntemente afetando o player
    /*
    Um dicionario é boa pq não queremos iterar por todos os elementos, e sim só consultar se dado elemento está incluído.
     O inteiro indica quantos efeitos daquele estão "estacados" no momento, e pode ser importante para lidar com o player tomar duas vezes o efeito da mesma poção
     
     */
    private Dictionary<PotionEffect, int> currentEffects;
    public ParticleSystem aura;


    void Start()
    {
        currentEffects = new Dictionary<PotionEffect, int>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool HasEffect(PotionEffect effect)
    {
        return currentEffects.ContainsKey(effect) && currentEffects[effect] > 0;
    }

    public void AddEffect(PotionEffect effect, float duration = 5.0f) //onde eu boto esse valor 5?
    {
        if (effect == PotionEffect.Nothing)
        {
            aura.Stop();
            return;
        }
        else
        {
            StartCoroutine(StartEffect(effect, duration));
            aura.Play();
        }
    }

    IEnumerator StartEffect(PotionEffect effect, float duration)
    {
        if(!currentEffects.ContainsKey(effect))
        {
            currentEffects[effect] = 1;
        }
        else
        {
            currentEffects[effect] = currentEffects[effect] + 1;
        }
        yield return new WaitForSecondsRealtime(duration);

        currentEffects[effect] = currentEffects[effect] - 1;
   
    }
}
