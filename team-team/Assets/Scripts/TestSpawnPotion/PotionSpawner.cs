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

    //J:array contendo obstaculos onde não é possivel o aparecimento das poções
        //acho que no final das contas, não é necessário esse array né? Ass: Krauss
    public GameObject[] Obstacles;

    //J:array contendo uma instância de cada tipo de poção diferente (ou várias, caso tenham probabilidades diferentes)
        //é uma boa já saber que vamos trabalhar com probabilidades, mas depois devemos pensar num jeito melhor de controlá-las, botar elementos repetidos na lista é meio ruim. Talvez pesos, sl. Ass: Krauss 
    private GameObject[] potionVariants;

    
    //variavel que armazena tamanho da poção, usado para verificar colisões durante o spawn
    public float potionSize;

    //J: PLACEHOLDER variavel de failsafe pra evitar que o editor crashe caso potionSize seja grande demais para adicionar todas as poções na região correta
    public int maxAttempts;
    //J: armazena Transform do objeto gerenciador do spawn para usar como base para rotação inicial da poção
    private Transform baseTransform;

    void Start()
    {
        potionVariants = GameController.Instance.potionVariants;
        
        baseTransform = GetComponent<Transform>();
        //J: cria o numero de poções igual ao máximo no inicio
        while (GameController.potionCount < maxPotions)
        {
            spawnPotion();
        }

    }
    
    void Update()
    {
        //J: reestoca poções no jogo
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

        //J: variavel que deve garantir que não entrará em loop infinito
        int attempts = 0;
        do {
            //J: escolhe aleatoriamente a poção que surgirá e a sua posição, dentro do limite definido
            chosenPotion = Random.Range(0, potionVariants.Length);
            spawnX = Random.Range(minSpawnX, maxSpawnX);
            spawnZ = Random.Range(minSpawnZ, maxSpawnZ);
            attempts++;
            //J: muda a posição caso exista um objeto dentro da area definida, a não ser que tenha tentadov vezes demais
        } while (Physics.CheckSphere(new Vector3(spawnX,SpawnY,spawnZ),potionSize,9)||attempts <= maxAttempts);
        Instantiate(potionVariants[chosenPotion], new Vector3(spawnX, SpawnY, spawnZ), baseTransform.rotation);
        GameController.potionCount += 1;
    }
}


