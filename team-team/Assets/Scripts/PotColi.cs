using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotColi : MonoBehaviour
{
    public enum FallStyle { linha, arco}
    //J: variaveis que armazenam o tipo da poção, quem a arremessou (-1 = ninguem), e se está sendo segurada, para impedir que a poção estoure na mão de quem carrega
    //K: mudei para um enum para coexistir com meu outro script
    [Header("Variaveis definindo características básicas desta poção")]
    [Tooltip("O tipo de efeito que esta poção tem")]
    public PotionEffect potionType;
    [Tooltip("O valor em pontos que esta poção tem, caso acerte o calderão, caso a condição de vitória sejam pontos!")]
    public int pointValueOnCauldron = 100;
    [Tooltip("O valor em pontos que esta poção tem, caso acerte outro player, caso a condição de vitória sejam pontos!")]
    public int pointValueOnPlayer = 100;
    [Tooltip("O nome do evento fmod a ser disparado quando a orbe impacta o player")]
    public string impactEventString;

    [Header("Ajuste das durações dos efeitos")]
    [Tooltip("Duração do efeito da poção")]
    public float potionDuration;

    private int thrower = -1;
    private bool thrown = false;

    //J: variavel para definir distancia atravessada pela orbe antes de cair, e modo de queda (1 = linear, 2 = fisico)
    [Header("Variaveis para definir alcance e forma de arremeço da orbe. Ver tooltips para mais infos")]
    [Tooltip("Variavel que define forma que a orbe cai")]
    public FallStyle fallType;
    [Tooltip("Variavel que define a altura que a orbe alcança, quando em queda do tipo arco, ou o tempo antes de cair, para queda em linha")]
    public int airTime;

    [Tooltip("Variável que define a velocidade base em que um objeto é refletido da parede")]
    public float ReflectionVelocityMultiplier = 2.0f;

    private Rigidbody rigidbody;
    private Collider collider;
    //armazenamos sempre qual o último objeto em que quicamos, para poder bloquear quiques duplos
    private GameObject lastObjectBouncedOn;

    private Vector3 currentVelocity;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        lastObjectBouncedOn = null;
        //K: uma solução simplista para o problema das orbes flutuando sozinhas, estranhamente: no inicio, desablitar qualquer movimento
        rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
    }

    void FixedUpdate()
    {
        //K: esse valor é necessário para calcular a reflexão, já que os efeitos da colisão sobre a física ocorrem *antes* da função OnCollisionEnter ser chamada!
        currentVelocity = rigidbody.velocity;
    }
    
    void Update()
    {
        if (thrown)
        {

            //K: esse método de contar o tempo faz sentido, mas acaba variando conforme o framerate do jogo
            airTime--;

            //J: se queda linear, liga gravidade quando tempo acabar
            if (fallType == FallStyle.linha)
            {
                if (airTime <= 0)
                {
                    this.GetComponent<Rigidbody>().useGravity = true;
                }
            }

            //J: se queda  em arco, faz cair.
            else if (fallType == FallStyle.arco)
            {
                    this.GetComponent<Rigidbody>().useGravity = true;
            }
            
        }

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
    public bool getThrown()
    {
        return thrown;
    }
    public void setThrown(bool newThrown)
    {
        thrown = newThrown;

        //J: se tiver trajetoria de arco, cria velocidade vertical
        if (newThrown && fallType == FallStyle.arco)
        {
            rigidbody.constraints = 0; //flag para nenhuma constraint 
            gameObject.GetComponent<Rigidbody>().AddForce(0, airTime, 0, ForceMode.VelocityChange);
        }

        
    }

    /*K:
    HitPlayer: função chamada quando a função deve "quebrar" em cima do player.
        - player é o player que deve sofrer os efeitos desta poção 

        - note que esta pontuação não está decrementando a potionCount
    */
    public void HitPlayer(PlayerEffects player)
    {
        player.AddEffect(potionType, potionDuration);

        //toca som de impacto com o player
        FMODUnity.RuntimeManager.PlayOneShot(impactEventString, transform.position);
        
        Throw _throw = player.gameObject.GetComponent<Throw>();

        Debug.Assert(_throw != null);
        //se quem arremessou é de outro time de quem foi acertado, 
        if(_throw.throwerTeam != this.getThrower())
        {
            //incrementa pontuação:
            GameController.Instance.AddPoints(pointValueOnPlayer, this.getThrower());
            _throw.barFill.fillAmount += 0.2f;
        }
        
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        
        Debug.Log("colidiu com: "+ collision.gameObject.name);
        if (collision.gameObject.CompareTag("Target"))
        {
            //J: destroi a poção ao colidir com o caldeirão
            Destroy(gameObject);
	    Destroy(collision.gameObject);
            GameController.potionCount--;
	    GameController.targetCount--;
            GameController.Instance.AddPoints(pointValueOnCauldron, this.getThrower());
            return;
        }   
        /*else if(thrown && collision.gameObject.CompareTag("Player"))
        {

            //J: atualiza contador de poções, para criar nova poção
            Destroy(gameObject);
            PlayerEffects pe = collision.gameObject.GetComponent<PlayerEffects>();
            Debug.Assert(pe != null);
            this.HitPlayer(pe);
            GameController.potionCount--;
            return;
        }*/
        else if (thrown && collision.gameObject.layer == LayerMask.NameToLayer("floor"))
        {
            //destruído ao colidir com chão
            Destroy(gameObject);
            GameController.potionCount--;
            return;
        }
        else if(thrown && collision.gameObject.layer == LayerMask.NameToLayer("reflector") && lastObjectBouncedOn != collision.gameObject)
        {
            lastObjectBouncedOn = collision.gameObject;
            //reflete trajetoria:
            //acha normal da superfície
            Bounds bounds = this.collider.bounds;
            float d = 4.0f*Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);
            RaycastHit hit;
            //Debug.DrawRay(bounds.center, d*(collision.GetContact(0).point - bounds.center).normalized, Color.red, 0.5f);
            bool bhit = Physics.Raycast(bounds.center, (collision.GetContact(0).point - bounds.center).normalized, out hit, d, ~LayerMask.NameToLayer("reflector"));
            if(bhit)
            {
                //Debug.DrawRay(hit.point, hit.normal*10, Color.green, 0.5f);
                //Debug.DrawRay(hit.point, currentVelocity*10, Color.blue, 0.5f);
                Vector3 refletido = Vector3.Reflect(currentVelocity, hit.normal.normalized);
                Debug.Log("refletido");
                //Debug.DrawRay(hit.point, refletido*10, Color.red, 0.5f);
                rigidbody.velocity = refletido.normalized * currentVelocity.magnitude * ReflectionVelocityMultiplier;
            }
            else
            {
                Debug.LogWarning("Raycast failed to find reflector surface on collision");
            }
            return;
            
        }
        else
        {
            //depois de colidir com qualquer outra coisa(paredes, poções), liga a gravidade se não estiver ligada
            rigidbody.useGravity = true;

        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger");
	
        if(thrown && other.CompareTag("Player"))
        {

            //J: atualiza contador de poções, para criar nova poção
            Destroy(gameObject);
            PlayerEffects pe = other.GetComponent<PlayerEffects>();
            Debug.Assert(pe != null);
            this.HitPlayer(pe);
            GameController.potionCount--;
        }
         
    }

}

