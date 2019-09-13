using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerInput))]
public class Move : MonoBehaviour
{
    [Header("Variáveis de customização do movimento do jogador. Ver tooltips para mais informações")]
    
    [Tooltip("A velocidade base(máxima) de movimentação do player")]
    public float moveSpeed = 5.0f;

    [Tooltip("O tempo em segundo que o player demora de velocidade 0 para velocidade máxima")]
    public float accelerationTime = 0.1f;
    [Tooltip("O tempo em segundo que o player demora de velocidade máxima para velocidade 0")]
    public float decelerationTime = 0.2f;
    [Tooltip("Determina se o jogador deve tentar rotacionar automaticamente quando é movido. Só é válido para joysticks, já que no teclado isso acontece de qualquer jeito")]
    public bool AutoRotate;
    [Tooltip("Caso ShouldAutoRotate esteja ativo, a velocidade base de rotação do jogador")]
    public float turnSpeed = 10.0f;

    private float angle;
    private Quaternion rot;

    //classe boba usada para centralizar configurações de controle do player. Acessada por esta classe pq isso afeta a movimentação
    private PlayerInput playerInput;

    private Vector3 move; //um vetor usado pra "passar input" da update pra fixed update
    private float currentSpeed; //um float usado pra manter a velocidade do player, irrespectivamente de direção

    private float yAcceleration; //não mexer. Usado pra SmoothDamp
    private float baseDeceleration; 
    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        //calcula coeficiente de desaceleração do player
        // aceleração = Vfinal - Vinicial / deltaTempo =>
        //desaceleração por frame = 0 - velocidadeMaxima / tempoDesaceleração
        baseDeceleration = (0 - moveSpeed) / decelerationTime;
        Debug.Assert(baseDeceleration < 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //Movimenta jogador
        //mudei as funções de Input.GetAxisRaw para InputManager.GetAxis, usando a classe que fiz para lidar com controles. Ass: Krauss
        float moveHon = InputManager.GetAxis(playerInput.controllerScheme, "HorizontalL");
        float moveVer = InputManager.GetAxis(playerInput.controllerScheme, "VerticalL");
        //Debug.Log(moveVer);
        Vector3 newMove = new Vector3(moveHon, 0.0f, moveVer);


        //calcula qual deve ser a velocidade do player
        if(newMove.sqrMagnitude > 0.1f)
        {
            //se player está inserindo input, ele tenta acelerar, até o limite da velocidade máxima
            move = newMove;
            currentSpeed = Mathf.SmoothDamp(currentSpeed, moveSpeed, ref yAcceleration, accelerationTime);
        }
        else
        {
            //senão, desacelera
            //V = V0 + at => Vframe = V(ultimoframe) + baseDeceleration * Time.deltaTime
            //repare que baseDeceleration já é negativo!
            currentSpeed = Mathf.Max(0.0f, currentSpeed + baseDeceleration * Time.deltaTime);
        }


        //move o player
        transform.position += move.normalized * Time.deltaTime * currentSpeed;

        
        //Calcula direção
        //angle = Mathf.Atan2(moveVer, moveHon);
        //angle = Mathf.Rad2Deg * angle;

        //angle += cam.eulerAngles.y; //não entendi o que exatamente a camera tem a ver com essa rotação. Esse valor é 0 de qquer forma Ass: Krauss
    
        //Rotaciona o jogador
        //rot = Quaternion.Euler(0, angle, 0);
        
        //se o player está usando um controle para jogar,
        if(playerInput.controllerScheme.mode == ControllerMode.Joystick)
        {
            float h = InputManager.GetAxis(playerInput.controllerScheme, "HorizontalR");
            float v = InputManager.GetAxis(playerInput.controllerScheme, "VerticalR");
            Vector2 input = new Vector2(h,v);
        

            //se o player está usando o analógico direito para se mexer, imediatamente vai para aquela orientação(por ora pelo menos)
            if(input.magnitude >= 0.6f) //botei um thrshold de 0.6 pra ele não ficar rodando a esmo e ficar sempre na direção do último input
            {
                float angle_rad = Mathf.Atan2(input.x, input.y);
                Quaternion rotation = new Quaternion();
                rotation.eulerAngles = new Vector3(0.0f, angle_rad * Mathf.Rad2Deg, 0.0f);
                transform.rotation = rotation;
            }
            else if(AutoRotate && move.magnitude > 0)
            {
                //auto-rotaciona
                rot = Quaternion.LookRotation(move, Vector3.up);
                //rotaciona jogador
                transform.rotation = Quaternion.Lerp(transform.rotation, rot, turnSpeed);    
        
            }
        }
        else if(move.magnitude > 0)
        {
            //se o player está controlando no teclado, a única maneira de rotacionar é autorotacionando.

            //calcula rotação
            rot = Quaternion.LookRotation(move, Vector3.up);
            //rotaciona jogador
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, turnSpeed);    
        
        }


    }

    void FixedUpdate()
    {
        
    }
}
