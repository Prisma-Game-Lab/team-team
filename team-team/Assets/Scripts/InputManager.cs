using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*

Script para abstrair a relação entre as componentes do jogo(movimentação dos jogadores, por exemplo) e
os controladores. Isso é positivo para que cada componente não precise se preocupar com problemas comuns do tipo trocar controle por teclado, controle desconectado etc.



Autores: Krauss,
 */

struct Joystick
{
    
}

public class InputManager : MonoBehaviour
{
    
    
    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Input.GetJoystickNames().Length);
        foreach (string str in Input.GetJoystickNames())
        {
            Debug.Log(str);
        }
        
    }


}
