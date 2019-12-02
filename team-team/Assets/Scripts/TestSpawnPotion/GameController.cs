using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public enum VictoryCondition
{
    //a primeira condição de vitória implementada: o primeiro time a completar a sequência específica de poções ganha
    PotionSequence,
    //uma condição de vitória simples, em que o primeiro time a alcançar N pontos ganha
    FirstToMaxPoints
}

public class GameController : MonoBehaviour
{
    //J: contador de poções existentes na cena
    public static int potionCount = 0;
    //J: contador de alvos existentes na cena
    public static int targetCount = 0;
    [Header("Configurações de Gameplay. Preste atenção nas tooltips")]

    [Tooltip("Controla a condição de vitória entre algumas opções. O valor deste item determina se algumas variáveis abaixo são ou não usadas")]
    //o setting atual para a condição de vitória
    public VictoryCondition victoryCondition;

    //J: numero de poções na sequencia necessária para vitória
    [Tooltip("numero de poções na sequencia necessária para vitória. Usado na condição PotionSequence")]
    public int objectiveSize;
    //K: o número de pontos para vitória, no caso FirstToMaxPoints
    [Tooltip("o número de pontos para vitória, no caso FirstToMaxPoints")]
    public float objectiveMaxPoints;



    [Header("Referências do Unity, cuidado ao mexer")]
    [Tooltip("lista de poções no jogo. aumente o tamanho da lista e adicione o prefab de uma poção para adicioná-la")]
    public GameObject[] potionVariants;
    [Tooltip("Tela de fim de jogo e sua mensagem.")]
    public GameObject telaFinal;
    private Text winner;
    

    [Tooltip("Prefab do alvo bonus")]
    public GameObject bonusTarget;
    [Tooltip("Objetos vazios que marcam as posições possíveis onde o alvo bonus pode aparecer")]
    public Transform[] targetSpawnPoints;
    [Tooltip("Intervalo entre alvos bonus")]
    public float targetCooldown;


    [Tooltip("número de equipes com pontuações separadas na cena")]
    //J: numero de times e pontuações atuais de cada time
    public int numTeams;
    //K: fiz de uma maneira em que agora há duas "pontuações" que são mantidas ao mesmo tempo. Uma é o quão longe o time foi no objetivo(de jogar N poções no poço) -> teamObjIndex
        //a outra é simplesmente uma contagem de pontos -> teamPoints
    public int[] teamPoints;
    private int[] teamObjIndex;

    //J: vetor que armazena o objetivo, gerado em Start()
    private static int[] objective;

    //J: grava se algum time ja ganhou
    private bool gameEnd = false;
    //K: grava qual time ganhou
    private int victoriousTeam;

    //J: referencias para texto exibido na UI
    public Text DisplayP1, DisplayP2, DisplayP3, DisplayP4, DisplayGoal;

    //J: String contendo os nomes dados para cada orbe, enquanto não temos imagens para apresentar no lugar na HUD
    public string[] orbNames;

    //K: lista de referencias para os players
    public List<GameObject> players;
    private int numPlayers;

    //J: variavel responsavel para receber a velocidade de arremesso base dos jogadores e atribui-la às poções para calcular alcance corretamente, para queda linear
    //K: prático, mas o que acontece se, por exemplo, players precisarem ter alcances diferentes quando lançam a poção? Por serem personagens diferentes ou pelo efeito de uma poção, por exemplo?
    private static float throwSpeedGlobal;

    //singleton stuff
    public static GameController Instance {get; private set;}

    //Barras super mago
    public Image[] playerBarFill;
    public SuperMago[] superMago;
    private CharacterSelectionData modelSelected;

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
        winner = telaFinal.GetComponentInChildren<Text>();
        Debug.Assert(potionVariants.Length > 0);
        //J: Inicializa texto do objetivo na UI
        Debug.Assert(DisplayGoal != null);
        if (victoryCondition == VictoryCondition.PotionSequence)
        {
            DisplayGoal.text = "Receita: ";
        }
        
        
        //J: gera aleatoriamente a sequencia necessária para vitoria
        objective = new int[objectiveSize];

        for(int i=0;i<objectiveSize; i++)
        {
            objective[i] = Random.Range(0, potionVariants.Length);
            
            DisplayGoal.text = DisplayGoal.text + orbNames[objective[i]];
            
        }
	    //J: inicia a função que cria os alvos bonus, que se repete sozinha
        StartCoroutine(spawnTarget(targetCooldown));

        //k: coisas temporárias pra setar o jogo de acordo com o num de players
        if(PersistentInfo.Instance != null)
        {
            //free for all
            numTeams = PersistentInfo.Instance.playersQtd;
            numPlayers = numTeams;
            modelSelected = PersistentInfo.Instance.PlayerData;
        }
        else
        {
            numTeams = 4;
            numPlayers = 4;
        }

        //K: desliga players que não estiverem jogando
        //supoe que o thrower team de cada player já está setado corretamente
        for(int i = 0; i < players.Count; i++)
        {
            if(players[i] != null)
            {
                players[i].transform.GetChild(modelSelected.CharSelected[i]).gameObject.SetActive(i<numPlayers);
                //players[i].SetActive(i < numPlayers);                
            }
        }

        //J: configura pontuação inicial de todos os times para 0
        teamPoints = new int[numTeams];
        teamObjIndex = new int[numTeams];
        for(int i=0;i<numTeams;i++)
        {
            teamPoints[i] = 0;
            teamObjIndex[i] = 0;
            playerBarFill[i].fillAmount = 0.0f;
            superMago[i].full = false;
        }

        //inicializa time vencedor para -1, no início da partida
        victoriousTeam = -1;

        //J: Inicializa texto da UI para cada jogador
        //K: isso acaba por não ser necessário aqui, porque já está sendo feito no update: mas isso pode ser revisto depois
            //não sei se fazer isso a cada update tem algum impacto na performance
        //UpdateScoringUI();


    }
    private void Update()
    {
        //J: atualiza o valor exibido como alvo atual de cada jogador
        UpdateScoringUI();
    }



    //J: função responsável por criar os alvos bonus com n segundos de intervalo.
    IEnumerator spawnTarget(float cooldown)
    {
	//enqanto o jogo não acabar
	while (!gameEnd)
	{
		//espera o tempo. pausa contagem se o jogo estiver pausado
        for(int i=0;i<cooldown;i++)
            {
                while (MenuPausa.pausado)
                {
                    yield return new WaitForSecondsRealtime(0.1f);
                }
                yield return new WaitForSecondsRealtime(1.0f);

            }

		//escolhe um spawn possivel
		int chosenSpawn = Random.Range(0,targetSpawnPoints.Length);
		
		//cria o alvo no spawn escolhido
		Debug.Log("Alvo Spawnado");
		Instantiate(bonusTarget,targetSpawnPoints[chosenSpawn].position,targetSpawnPoints[chosenSpawn].rotation);
		targetCount++;

		//espera o alvo sumir;
		yield return new WaitUntil(() => targetCount == 0);
	}
    }

    

    // informa se o time em questão ganhou o jogo!
    //varia de acordo com o objetivo corrente
    private bool HasWonGame(int teamIndex)
    {
        switch(victoryCondition)
        {
            case VictoryCondition.PotionSequence:
                return teamObjIndex[teamIndex] >= objectiveSize;
            case VictoryCondition.FirstToMaxPoints:
                return teamPoints[teamIndex] >= objectiveMaxPoints;
            default:
                Debug.LogError("Victory condition not implemented!");
                return false;
        }
    }

    //J: atualiza o valor exibido como alvo atual de cada jogador
    private void UpdateScoringUI()
    {
        if(gameEnd)
        {
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
            telaFinal.SetActive(true);
            telaFinal.GetComponent<TelaFinal>().ActivateTelaFinal();
            //winner.text = "Jogador " + (victoriousTeam+1).ToString() + " ganhou!";
            DisplayGoal.enabled = true;
            
            return;
        }
        //else:
        if(victoryCondition == VictoryCondition.PotionSequence)
        {
            DisplayGoal.enabled = true;
            if (DisplayP1 != null && !gameEnd)
            {
                DisplayP1.text = "Alvo do jogador 1:" + orbNames[objective[teamObjIndex[0]]].ToString();
            }
            if (DisplayP2 != null && !gameEnd)
            {
                DisplayP2.text = "Alvo do jogador 2:" + orbNames[objective[teamObjIndex[1]]].ToString();
            }
            if (DisplayP3 != null && !gameEnd)
            {
                DisplayP3.text = "Alvo do jogador 3:" + orbNames[objective[teamObjIndex[2]]].ToString();
            }
            if (DisplayP4 != null && !gameEnd)
            {
                DisplayP4.text = "Alvo do jogador 4:" + orbNames[objective[teamObjIndex[3]]].ToString();
            }
        }
        else if(victoryCondition == VictoryCondition.FirstToMaxPoints)
        {
            //desliga display de ordem das poções caso não estejamos usando esta condição de vitória!
            DisplayGoal.enabled = false;
            
            //K: como seria uma maneira mais limpa de fazer essa repetição
            if (DisplayP1 != null)
            {
                DisplayP1.text = teamPoints[0] + " " ;
            }
            if (DisplayP2 != null)
            {
                DisplayP2.text = teamPoints[1] + " " ;
            }
            if (DisplayP3 != null && teamPoints.Length > 2)
            {
                DisplayP3.text = "Jogador 3:" + teamPoints[2];
            }
            if (DisplayP4 != null && teamPoints.Length > 3)
            {
                DisplayP4.text = "Jogador 4:" + teamPoints[3];
            }
        }
        



    }

    private void OnTriggerEnter(Collider other)
    {

        //se o objeto colisor for uma poção, trata dos efeitos que ela terá (basicamente contar pontuação)
        PotColi potion = other.GetComponent<PotColi>();
        if(potion != null) ReceivePotion(potion);
    }

    //"recebe" uma poção no calderão e ajusta a pontuação de acordo
    private void ReceivePotion(PotColi potion)
    {
        int potionType = (int) potion.potionType;
        int throwerTeam = potion.getThrower();

        if(gameEnd || throwerTeam < 0) return;
        switch(victoryCondition)
        {
            case VictoryCondition.PotionSequence:

                //K: por enquanto está sendo feito este cast de enum pra int, o que é válido e seguro. Porém, seria mais organizado mudar tudo logo
                //J: se a orbe jogada é o objetivo atual do time, marca ponto. Também impede erros caso uma orbe spawne dentro do poço
                if(potionType == objective[teamObjIndex[throwerTeam]])
                if(potionType == objective[teamObjIndex[throwerTeam]])
                {
                    teamObjIndex[throwerTeam]++;
                    Debug.Log("team " + throwerTeam + " scores!");
                }                
                break;
            
            case VictoryCondition.FirstToMaxPoints:
                teamPoints[throwerTeam] += potion.pointValueOnCauldron;
                break;
            
            default:
                Debug.LogError("Victory condition not implemented!");
                break;
        }

        if(HasWonGame(throwerTeam))
        {
            Debug.Log("team " + throwerTeam + " wins!");
            gameEnd = true;
            victoriousTeam = throwerTeam;
        }
    }

    public void AddPoints(int points, int team)
    {
        //K: talvez verificar aqui se o modo de jogo está de fato usando pontos?
        if(team >= 0 && team < numTeams)
        {
            if(superMago[team].full == false)
            {
                playerBarFill[team].fillAmount += 0.1f;
            }
            teamPoints[team] += points;
            if(HasWonGame(team))
            {
                Debug.Log("team " + team + " wins!");
                gameEnd = true;
                victoriousTeam = team;
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
