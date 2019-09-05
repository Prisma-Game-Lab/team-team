using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //contador de poções existentes na cena
    public static int potionCount = 0;

    //numero de poções na sequencia necessária para vitória
    public int objectiveSize;

    //variações de poções disponiveis
    public GameObject[] potionVariants;

    //numero de times e pontuações atuais de cada time
    public int numTeams;
    private int[] teamScores;

    //vetor que armazena o objetivo, gerado em Start()
    private static int[] objective;

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
        
        //gera aleatoriamente a sequencia necessária para vitoria
        objective = new int[objectiveSize];

        Debug.Log("Objetivo: ");
        for(int i=0;i<objectiveSize; i++)
        {
            objective[i] = Random.Range(0, potionVariants.Length);
            Debug.Log(objective[i]);
        }

        //configura pontuação inicial de todos os times para 0
        teamScores = new int[numTeams];
        for(int i=0;i<numTeams;i++)
        {
            teamScores[i] = 0;
        }
    }
    private void Update()
    {
        //busca a cada frame se algum time completou o objetivo
        for (int i = 0; i < numTeams; i++)
        {
            if (teamScores[i] >= objectiveSize)
            {
                Debug.Log("team " + i + "wins!");
                break;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.CompareTag("potion_placeholder"))
        {
            //PLACEHOLDER: deve receber valor do time do jogador que arremessou a poção e do tipo da poção.
            int throwerTeam = 0;
            int thrownPotion = 0;

            //se a poção jogada é o objetivo atual do time, marca ponto
            if (thrownPotion==objective[teamScores[throwerTeam]])
            {
                teamScores[throwerTeam]++;
                Debug.Log("team " + throwerTeam + "scores!");
            }
        }
    }
    public static int[] getObjective()
    {
        //retorna a lista de objetivos para o exterior. util para quando for implementar a hud
        return GameController.objective;
    }



}
