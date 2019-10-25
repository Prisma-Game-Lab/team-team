using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Throw: script que implementa o comportamento de um player segurar objetos e poder arremessá-los

Autores: Arthur, Krauss, Arthus James

 */
public class Throw : MonoBehaviour
{    
    [Header("Referências do Unity(não mexer, só se você souber o que está fazendo)")]
    public Transform holdpoint;
    [HideInInspector] public bool holding = false;

    [Header("Variáveis de ajuste da gameplay. Ver tooltips para mais infos")]
    [Tooltip("Variável para ajustar a velocidade em que o jogador arremessa a poção")]
    public float throwSpeed = 100.0f;
    [Tooltip("variavel que determina a qual time o jogador pertence (começa contagem do 0)")]
    //J: , e o script da poção com que se está interagindo
    public int throwerTeam;

    [Tooltip("O nome do evento/som a ser tocado quando a orbe é arremessada")]
    public string throwEventString = "event:/Efeitos/personagens/arremesso potion";

    //Arthur, é de fato importante ter essa variável Rigidbody aqui. Mas é importante que o nome dela seja potionRigidbody, ou algo mais descritivo que só rb. Tomei a liberdade de mudar. Ass: Krauss
        //Além disso, é importante que ela seja setada quando o player pega uma poção, senão ele só poderia pegar uma única poção durante o jogo
    private Rigidbody potionRigidbody;
    

    
    private PotColi potionScript = null;

    //classe bobinha que armazena configuração de controle do jogador
    private PlayerInput playerInput;
    private PlayerEffects playerEffects;
    private Transform transform;
    private Collider trigger;

    //O: Variável que define se o jogador pode jogar ou não os itens
    private bool canThrow = true;

    // Start is called before the first frame update
    void Start()
    {
        GameController.setThrowSpeedGlobal(throwSpeed);
        playerInput = this.GetComponent<PlayerInput>();
        playerEffects = GetComponent<PlayerEffects>();
        transform = GetComponent<Transform>();
        trigger = GetComponent<Collider>();

        Debug.Assert(playerEffects != null);
    }

    // Update is called once per frame
    void Update()
    {
        //se o personagem está segurando uma poção,
        if (holding)
        {
            GetCanThrow();
            //Debug.Assert(potionRigidbody != null);
            //J: Corrige o erro de ficar impedido de pegar poção nova
            //K: não é melhor setar pra null toda vez que arremessar a poção?
            if (potionRigidbody == null)
            {
                holding = false;
            }
            else
            {
                //seta a posição da poção
                potionRigidbody.transform.position = holdpoint.position;
                potionRigidbody.useGravity = false;

                if (InputManager.GetKeyDown(playerInput.controllerScheme, "Action1") && canThrow)
                {
                    //arremessa a poção que estava sendo segurada


                    //K: remove contraints de movimento, para arremessar a poção
                    potionRigidbody.constraints = 0;

                    //K: O, eu alterei esta linha abaixo para fazer com que o objeto seja arremessado em várias direções, e não só para direita!
                    Vector3 throwDirection = transform.forward;
                    potionRigidbody.velocity = throwDirection * throwSpeed;
                    potionRigidbody.gameObject.transform.SetParent(null);
                    //potionRigidbody.useGravity = true;
                    holding = false;

                    //J: altera o valor Thrown da poção para true
                    potionScript.setThrown(true);

                    //K: a propria poção faz isso, quando ela sai do trigger do player
                    //potionRigidbody.gameObject.GetComponent<Collider>().enabled = true;

                    //K: começa uma corrotina improvisada, para só reativar o collider da orbe quando ela já estiver longe o suficiente deste player
                    StartCoroutine(OrbIsOutside(potionRigidbody));

                    //emite som de arremesso de poção
                    FMODUnity.RuntimeManager.PlayOneShot(throwEventString, transform.position);
                    
                }
                else if(InputManager.GetKeyDown(playerInput.controllerScheme, "Action2") && canThrow)
                {
                    //joga a poção em si mesmo!
                    GameController.potionCount--;
                    potionScript.HitPlayer(playerEffects);
                }
            }
            
        }
            
    }

    //coroutina responsável por esperar a orbe sair de perto do player pra depois reativar seu collider
    private IEnumerator OrbIsOutside(Rigidbody Orbrigidbody)
    {
        Transform Orbtransform = Orbrigidbody.gameObject.GetComponent<Transform>();
        yield return new WaitUntil(() => !trigger.bounds.Contains(Orbtransform.position));
        yield return new WaitForSeconds(0.1f); //K: meio gambiarra, mas é a vida(pré-hacktudo)
        Orbrigidbody.gameObject.GetComponent<Collider>().enabled = true;
        yield break;
    }

    // on TRIGGER enter
    private void OnTriggerEnter(Collider other)
    {
        //se o player colidir com a poção && o player não estiver segurando outra poção,
        PotColi pc = other.GetComponent<PotColi>();
        if(other.gameObject.CompareTag("Potion") && holding == false && pc != null && !other.GetComponent<PotColi>().getThrown())
        {
            //segura esta poção:
            holding = true;
            other.gameObject.transform.SetParent(holdpoint);
            other.gameObject.transform.position = holdpoint.position;
            potionRigidbody = other.gameObject.GetComponent<Rigidbody>();
            //K: pra que mexer no isKinematic?
            potionRigidbody.isKinematic = false;

            potionRigidbody.gameObject.GetComponent<Collider>().enabled = false;

            //J: adquire script da poção arremessada e altera o valor de qual time carrega/arremessa a poção
            potionScript = pc;
            potionScript.setThrower(throwerTeam);
        }
    }

    //O: Função que altera o valor da variável canThrow caso o jogador esteja ou não congelado
    private bool GetCanThrow()
    {
        bool freeze = playerEffects.HasEffect(PotionEffect.Freeze);
        if(freeze)
        {
            canThrow = false;
        }
        else
        {
            canThrow = true;
        }
        return canThrow;

        //K: não há nada de errado com a estrutura escrita acima(pode ser útil mais pra frente pra implementar efeitos visuais e tal,)
        //mas repare que do jeito que está hoje, ela poderia ser simplificada para: 
            // return  !playerEffects.HasEffect(PotionEffect.Freeze);
    }
}
