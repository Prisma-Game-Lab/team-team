using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Classe para centralizar configurações de controles por cada jogador. Por enquanto não tem muita utilidade além
de armazenar uma variável ControllerScheme


Autores: Krauss,
 */

public class PlayerInput : MonoBehaviour
{
    
    [Header("Configurações de controlador para este player")]
    [Tooltip("Determina se este player usa joystick ou teclado, e qual o seu índice")]
    [SerializeField] private ControllerScheme _controllerScheme;
    public ControllerScheme controllerScheme
    {
        get{ return _controllerScheme; }
    }

}
