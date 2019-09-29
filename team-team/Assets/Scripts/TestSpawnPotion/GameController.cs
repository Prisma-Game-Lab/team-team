using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    //J: referencias para texto exibido na UI
    public Text DisplayP1, DisplayP2, DisplayP3, DisplayP4, DisplayGoal;

    //J: String contendo os nomes dados para cada orbe, enquanto não temos imagens para apresentar no lugar na HUD
    public string[] orbNames;

    //J: variavel responsavel para receber a velocidade de arremesso base dos jogadores e atribui-la às poções para calcular alcance corretamente, para queda linear
    //K: prático, mas o que acontece se, por exemplo, players precisarem ter alcances diferentes quando lançam a poção? Por serem personagens diferentes ou pelo efeito de uma poção, por exemplo?
    private static float throwSpeedGlobal;

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
        //J: Inicializa texto do objetivo na UI
        Debug.Assert(DisplayGoal != null);
        DisplayGoal.text = "Receita: ";

        
        //J: gera aleatoriamente a sequencia necessária para vitoria
        objective = new int[objectiveSize];

        Debug.Log("Objetivo: ");
        for(int i=0;i<objectiveSize; i++)
        {
            objective[i] = Random.Range(0, potionVariants.Length);
            Debug.Log("Poção "+ i + ": " + objective[i]);
            
            DisplayGoal.text = DisplayGoal.text + orbNames[objective[i]];
            
        }

        //J: configura pontuação inicial de todos os times para 0
        teamScores = new int[numTeams];
        for(int i=0;i<numTeams;i++)
        {
            teamScores[i] = 0;
        }

        //J: Inicializa texto da UI para cada jogador

        if (DisplayP1 != null)
        {
            DisplayP1.text = "Alvo do jogador 1:";
        }
        if (DisplayP2 != null)
        {
            DisplayP2.text = "Alvo do jogador 2:";
        }
        if (DisplayP3 != null)
        {
            DisplayP3.text = "Alvo do jogador 3:";
        }
        if (DisplayP4 != null)
        {
            DisplayP4.text = "Alvo do jogador 4:";
        }


    }
    private void Update()
    {
        //J: atualiza o valor exibido como alvo atual de cada jogador

        if (DisplayP1 != null && !gameEnd)
        {
            DisplayP1.text = "Alvo do jogador 1:" + orbNames[objective[teamScores[0]]].ToString();
        }
        if (DisplayP2 != null && !gameEnd)
        {
            DisplayP2.text = "Alvo do jogador 2:" + orbNames[objective[teamScores[1]]].ToString();
        }
        if (DisplayP3 != null && !gameEnd)
        {
            DisplayP3.text = "Alvo do jogador 3:" + orbNames[objective[teamScores[2]]].ToString();
        }
        if (DisplayP4 != null && !gameEnd)
        {
            DisplayP4.text = "Alvo do jogador 4:" + orbNames[objective[teamScores[3]]].ToString();
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.CompareTag("Potion"))
        {
            int throwerTeam = other.GetComponent<PotColi>().getThrower();
            int thrownPotion = other.GetComponent<PotColi>().potionType;

            
            //J: se a orbe jogada é o objetivo atual do time, marca ponto. Também impede erros caso uma orbe spawne dentro do poço
            
            if (!gameEnd && throwerTeam != -1 && thrownPotion==objective[teamScores[throwerTeam]])
            {
                teamScores[throwerTeam]++;
                Debug.Log("team " + throwerTeam + " scores!");

                //J: verifica se o time que pontuou completou o objetivo
                if(teamScores[throwerTeam]>=objectiveSize)
                {
                    Debug.Log("team " + throwerTeam + " wins!");
                    gameEnd = true;

                    if (DisplayP1 != null)
                    {
                        DisplayP1.text = "";
                        DisplayP1 = null;
                    }
                    if (DisplayP2 != null)
                    {
                        DisplayP2.text = "";
                        DisplayP2 = null;
                    }
                    if (DisplayP3 != null)
                    {
                        DisplayP3.text = "";
                        DisplayP3 = null;
                    }
                    if (DisplayP4 != null)
                    {
                        DisplayP4.text = "";
                        DisplayP4 = null;
                    }
                    throwerTeam++;
                    DisplayGoal.text = "Jogador " + throwerTeam.ToString() + " ganhou!";
                }
            }
        }
    }
    public static int[] getObjective()
    {
        //J: retorna a lista de objetivos para o exterior. util para quando for implementar a hud
        return GameController.objective;
    }

    public static float getThrowSpeedGlobal()
    {
        return GameController.throwSpeedGlobal;
    }
    public static void setThrowSpeedGlobal(float newSpeed)
    {
        throwSpeedGlobal = newSpeed;
    }


}
