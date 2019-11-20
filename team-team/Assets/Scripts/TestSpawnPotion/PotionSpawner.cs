using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script criado por arthus para gerenciar o aparecimento de poções no jogo

public class PotionSpawner : MonoBehaviour
{
    [Header("Configurações de quantidade de poções")]

    //J: contador inicial e final de orbes na cena, para incrementar com o tempo, e intervalo entre incrementos.
    [Tooltip("Número máximo de orbes inicial")]
    public int initialMaxOrbs;
    [Tooltip("Número máximo de orbes ao final do jogo")]
    public int finalMaxOrbs;
    [Tooltip("Intervalo entre alterações no número máximo de orbes, em segundos")]
    public float IncrementInterval;
    //valor que gerencia a quantidade maxima de poções existentes
    private int currentMaxOrbs;

    //valores que definem os extremos da região permitida ao aparecimento das poções
    public float minSpawnX;
    public float maxSpawnX;
    public float minSpawnZ;
    public float maxSpawnZ;
    public float SpawnY;

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
        currentMaxOrbs = initialMaxOrbs;
        potionVariants = GameController.Instance.potionVariants;
        
        baseTransform = GetComponent<Transform>();
        //J: cria o numero de poções igual ao máximo no inicio
        while (GameController.potionCount < currentMaxOrbs)
        {
            spawnPotion();
        }
        StartCoroutine(potionCountIncrement());

    }
    
    void Update()
    {
        //J: reestoca poções no jogo
        if (GameController.potionCount < currentMaxOrbs)
        {
            spawnPotion();
            Debug.Log("Spawn");
        }
    }

    void spawnPotion()
    {
        int chosenPotion = 0;
        float spawnX = 0.0f;
        float spawnZ = 0.0f;
        //J: variavel que deve garantir que não entrará em loop infinito
        int attempts = 0;
        Collider[] overlaps;
        do {
            //J: escolhe aleatoriamente a poção que surgirá e a sua posição, dentro do limite definido
            chosenPotion = Random.Range(0, potionVariants.Length);
            spawnX = Random.Range(minSpawnX, maxSpawnX);
            spawnZ = Random.Range(minSpawnZ, maxSpawnZ);
            attempts++;
            overlaps = Physics.OverlapSphere(new Vector3(spawnX, SpawnY, spawnZ), potionSize);
            //J: muda a posição caso exista um objeto dentro da area definida, a não ser que tenha tentadov vezes demais
        } while (overlaps.Length != 0 && attempts < maxAttempts);
        if (attempts < maxAttempts)
        {
            Instantiate(potionVariants[chosenPotion], new Vector3(spawnX, SpawnY, spawnZ), baseTransform.rotation);
        }
        //J: soma mesmo que não instancie, para terminar o loop caso não tenham espaços disponiveis para spawnar orbes
        GameController.potionCount += 1;
    }
    IEnumerator potionCountIncrement()
    {
        if (initialMaxOrbs < finalMaxOrbs)
        {
            while (currentMaxOrbs < finalMaxOrbs)
            {
                yield return new WaitForSecondsRealtime(IncrementInterval);
                currentMaxOrbs++;
                Debug.Log("Incrementado");
            }
        }
        else
        {
            while (currentMaxOrbs > finalMaxOrbs)
            {

                yield return new WaitForSecondsRealtime(IncrementInterval);
                currentMaxOrbs--;
                Debug.Log("Decrementado");
            }
        }
    }
}



