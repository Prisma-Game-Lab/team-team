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

    private Rigidbody rigidbody;
    private Collider collider;
    private float raycastDistance = 0.5f;
    //armazenamos sempre qual o último objeto em que quicamos, para poder bloquear quiques duplos
    private GameObject lastObjectBouncedOn;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        lastObjectBouncedOn = null;
    }
    
    void Update()
    {
        if (thrown )
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

        
        Throw _throw = player.gameObject.GetComponent<Throw>();
        Debug.Assert(_throw != null);
        //se quem arremessou é de outro time de quem foi acertado, 
        if(_throw.throwerTeam != this.getThrower())
        {
            //incrementa pontuação:
            GameController.Instance.AddPoints(pointValueOnPlayer, this.getThrower());
        }
        
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject);
        if (other.gameObject.CompareTag("Target"))
        {
            //J: destroi a poção ao colidir com o caldeirão
            Destroy(gameObject);
            GameController.potionCount--;
        }   
        else if(thrown && other.gameObject.CompareTag("Player"))
        {

            //J: atualiza contador de poções, para criar nova poção
            Destroy(gameObject);
            PlayerEffects pe = other.gameObject.GetComponent<PlayerEffects>();
            Debug.Assert(pe != null);
            this.HitPlayer(pe);
            GameController.potionCount--;
        }
        else if (thrown && other.gameObject.layer == LayerMask.NameToLayer("floor"))
        {
            //destruído ao colidir com chão
            Destroy(gameObject);
            GameController.potionCount--;
        }
        else if(thrown && lastObjectBouncedOn != other.gameObject)
        {
            Debug.Log("BOING");
            //reflete trajetoria
            lastObjectBouncedOn = other.gameObject;
            this.GetComponent<Rigidbody>().velocity = Vector3.Reflect(this.GetComponent<Rigidbody>().velocity, other.contacts[0].normal);
        }
    }

    /*private void OnTriggerEnter(Collider other)
    {
        //K: seria válido testar aqui se a poção foi arremessada? thrown?
        if (other.gameObject.CompareTag("Target"))
        {
            //J: destroi a poção ao colidir com o caldeirão
            Destroy(gameObject);
            GameController.potionCount--;
        }   
        else if(thrown && other.CompareTag("Player"))
        {

            //J: atualiza contador de poções, para criar nova poção
            Destroy(gameObject);
            PlayerEffects pe = other.GetComponent<PlayerEffects>();
            Debug.Assert(pe != null);
            this.HitPlayer(pe);
            GameController.potionCount--;
        }
        else if (thrown && other.gameObject.layer == LayerMask.NameToLayer("floor"))
        {
            //destruído ao colidir com chão
            Destroy(gameObject);
            GameController.potionCount--;
        }
        else if(thrown && lastObjectBouncedOn != other.gameObject)
        {

            
            Debug.Log("quica" + lastObjectBouncedOn.ToString());
            //quica ao colidir com outras coisas
            //outros orbes???
            
            //direções em que devo checar colisões
            int hDir = rigidbody.velocity.x > 0 ? 1 : -1;
            int vDir = rigidbody.velocity.z > 0 ? 1 : -1;

            Vector3 newVelocity = rigidbody.velocity;
            RaycastHit hit;
            //há 8 origens para o raycast, uma para cada canto do bound
            Vector3[] origins = new Vector3[8];
            float dx = collider.bounds.extents.x; float dy = collider.bounds.extents.y; float dz = collider.bounds.extents.z;
            origins[0] = collider.bounds.center + new Vector3(dx, dy, dz);
            origins[1] = collider.bounds.center + new Vector3(dx, dy, -dz);
            origins[2] = collider.bounds.center + new Vector3(dx, -dy, dz);
            origins[3] = collider.bounds.center + new Vector3(dx, -dy, -dz);
            origins[4] = collider.bounds.center + new Vector3(-dx, dy, dz);
            origins[5] = collider.bounds.center + new Vector3(-dx, dy, -dz);
            origins[6] = collider.bounds.center + new Vector3(-dx, -dy, dz);
            origins[7] = collider.bounds.center + new Vector3(-dx, -dy, -dz);
            
            //há colisores nas direções de movimento? se sim, inverto minha velocidade nessa direção -> quico
            //horizontal, ou x
            foreach(Vector3 origin in origins)
            {
                Debug.DrawRay(origin, Vector3.right * hDir * raycastDistance, Color.red, 2);
                if(other.Raycast(new Ray(this.transform.position, Vector3.right * hDir), out hit, raycastDistance))
                {
                    newVelocity.x = -newVelocity.x;
                    Debug.Log("Invert x!");
                    break;
                }
            }
            
            //"vertical", ou z
            foreach(Vector3 origin in origins)
            {
                Debug.DrawRay(origin, Vector3.forward * vDir * raycastDistance, Color.blue, 2);
                if(other.Raycast(new Ray(this.transform.position, Vector3.forward * vDir), out hit, raycastDistance))
                {
                    newVelocity.z = -newVelocity.z;
                    Debug.Log("Invert z!");
                    break;
                }
            }
            

            rigidbody.velocity = newVelocity;

            //verificar se é necessário botar algum tipo de cooldown no quique
            
        }
        
    }*/
}
