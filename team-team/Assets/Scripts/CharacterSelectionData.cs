using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionData : ScriptableObject
{
    [HideInInspector] public int[] PlayerController; //guarda o número do controle que controla cada mago. -1 por default.
    [HideInInspector] public int[] CharSelected; //guarda o número do personagem que cada player selecionou.
    [HideInInspector] public int[] PlayerIndex; //guarda o números dos players que serão mostrados na UI (vai de 1 até 4).
}
