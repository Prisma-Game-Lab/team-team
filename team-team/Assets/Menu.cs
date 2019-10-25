using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private CharSelection charSelection;
    public void Iniciar()
    {
        if(charSelection.CheckIfPlayersReady())
            SceneManager.LoadScene("Main");
    }

    public void Sair()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
