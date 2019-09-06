using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //J: contador de poções existentes na cena
    public static int potionCount = 0;

    //J: numero de poções na sequencia necessária para vitória
    public int objectiveSize;

    //J: variações de poções disponiveis
    public GameObject[] potionVariants;

    //J: numero de times e pontuações atuais de cada time
    public int numTeams;
    private int[] teamScores;

    //J: vetor que armazena o objetivo, gerado em Start()
    private static int[] objective;

    //J: grava se algum time ja ganhou
    private bool gameEnd = false;
    //singleton stuff
    public static GameController Instance {get; private set;}
    void Awake()
    {
        //se não há alguma outra instancia desta classe, me registro
        if(Instance == null)
        {
            Instance = this;
        }
        //se há alguma outra já registrada, me destruo
        else
        {
            GameObject.Destroy(this);
        }
        //desta maneira, sempre só haverá 1 GameController na cena(padrão singleton)
    }



    void Start()
    {
        Debug.Assert(potionVariants.Length > 0);
        
        //J: gera aleatoriamente a sequencia necessária para vitoria
        objective = new int[objectiveSize];

        Debug.Log("Objetivo: ");
        for(int i=0;i<objectiveSize; i++)
        {
            objective[i] = Random.Range(0, potionVariants.Length);
            Debug.Log("Poção "+ i + ": " + objective[i]);
        }

        //J: configura pontuação inicial de todos os times para 0
        teamScores = new int[numTeams];
        for(int i=0;i<numTeams;i++)
        {
            teamScores[i] = 0;
        }
    }
    private void Update()
    {
        //J: busca a cada frame se algum time completou o objetivo
        for (int i = 0; i < numTeams; i++ )
        {
            if (teamScores[i] >= objectiveSize && !gameEnd)
            {
                Debug.Log("team " + i + " wins!");
                gameEnd = true;
                break;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.CompareTag("Potion"))
        {
            int throwerTeam = other.GetComponent<PotColi>().getThrower();
            int thrownPotion = other.GetComponent<PotColi>().potionType;

            //J: se a poção jogada é o objetivo atual do time, marca ponto
            if (!gameEnd && thrownPotion==objective[teamScores[throwerTeam]])
            {
                teamScores[throwerTeam]++;
                Debug.Log("team " + throwerTeam + " scores!");
            }
        }
    }
    public static int[] getObjective()
    {
        //J: retorna a lista de objetivos para o exterior. util para quando for implementar a hud
        return GameController.objective;
    }



}
