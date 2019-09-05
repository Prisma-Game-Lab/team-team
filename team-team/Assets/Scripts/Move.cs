using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float turnSpeed = 10.0f;
    float angle;
    Quaternion rot;
    Transform cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Movimenta jpgador
        float moveHon = Input.GetAxisRaw("Horizontal");
        float moveVer = Input.GetAxisRaw("Vertical");
        Vector3 move = new Vector3(moveHon, 0.0f, moveVer);
        transform.position += move * Time.deltaTime * moveSpeed;

        //Mantem posição
        if (Mathf.Abs(moveHon) < 1 && Mathf.Abs(moveVer) < 1)
        {
            return;
        }

        
        //Calcula direção
        //angle = Mathf.Atan2(moveVer, moveHon);
        //angle = Mathf.Rad2Deg * angle;

        //angle += cam.eulerAngles.y; //não entendi o que exatamente a camera tem a ver com essa rotação. Esse valor é 0 de qquer forma Ass: Krauss
    
        //Rotaciona o jogador
        //rot = Quaternion.Euler(0, angle, 0);
        
        //calcula rotação
        rot = Quaternion.LookRotation(move, Vector3.up);
        //rotaciona jogador
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, turnSpeed);


    }
}
