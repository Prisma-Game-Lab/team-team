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
            return;
     
        //Calcula direção
        angle = Mathf.Atan2(moveHon, moveVer);
        angle = Mathf.Rad2Deg * angle;
        angle += cam.eulerAngles.y;
    
        //Rotaciona o jogador
        rot = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, turnSpeed);
    }
}
