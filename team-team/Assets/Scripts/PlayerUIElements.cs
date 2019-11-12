using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Classe para posicionar e rotacionar elementos de HUD que acompanham o personagem.


Autores: Gorette,
 */

public class PlayerUIElements : MonoBehaviour
{
    // Variável pública do tipo GameObject para que o objeto do jogador seja escolhido no Editor.
    public GameObject player;
    public GameObject holdpoint;
    private Vector3 newPosition;
    private Vector3 offset;
    private float rot;

    void Start()
    {
        
        newPosition = new Vector3(player.transform.position.x, 0.55f, player.transform.position.z);
        transform.position = newPosition;
        offset = transform.position - player.transform.position;

        //transform.Rotate(-90f, 0.0f, 0.0f, Space.Self);
        rot = holdpoint.transform.eulerAngles.y;
        transform.eulerAngles = new Vector3(-90, rot, 0);
    }
    
    void LateUpdate()
    {
       
        transform.position = player.transform.position + offset;

        rot = holdpoint.transform.eulerAngles.y;
        transform.eulerAngles = new Vector3(-90, rot, 0);
    }

}
