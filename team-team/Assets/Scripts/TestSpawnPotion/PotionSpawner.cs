using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script criado por arthus para gerenciar o aparecimento de poções no jogo

public class PotionSpawner : MonoBehaviour
{
    //valor que gerencia a quantidade maxima de poções existentes
    public int maxPotions;

    //valores que definem os extremos da região permitida ao aparecimento das poções
    public float minSpawnX;
    public float maxSpawnX;
    public float minSpawnZ;
    public float maxSpawnZ;
    public float SpawnY;

    //array contendo obstaculos onde não é possivel o aparecimento das poções
    public GameObject[] Obstacles;

    //array contendo uma instância de cada tipo de poção diferente (ou várias, caso tenham probabilidades diferentes)
    public GameObject[] potionVariants;

    //PLACEHOLDER: variavel que armazena tamanho da poção, usado para verificar colisões durante o spawn
    public float potionSize;

    //PLACEHOLDER: armazena Transform do GameManager para usar como base para rotação inicial da poção
    private Transform baseTransform;
    void Start()
    {
        baseTransform = GetComponent<Transform>();
        //cria o numero de poções igual ao máximo no inicio
        while (GameController.potionCount < maxPotions)
        {
            spawnPotion();
        }

    }
    
    void Update()
    {
        //reestoca poções no jogo
        if (GameController.potionCount < maxPotions)
        {
            spawnPotion();
        }
    }

    void spawnPotion()
    {
        int chosenPotion = 0;
        float spawnX = 0.0f;
        float spawnZ = 0.0f;
        do {
            //escolhe aleatoriamente a poção que surgirá e a sua posição, dentro do limite definido
            chosenPotion = Random.Range(0, potionVariants.Length);
            spawnX = Random.Range(minSpawnX, maxSpawnX);
            spawnZ = Random.Range(minSpawnZ, maxSpawnZ);

        } while (Physics.CheckSphere(new Vector3(spawnX,SpawnY,spawnZ),potionSize));

        Instantiate(potionVariants[chosenPotion], new Vector3(spawnX, SpawnY, spawnZ), baseTransform.rotation);
        GameController.potionCount += 1;
    }
}


