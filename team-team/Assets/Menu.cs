using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Iniciar()
    {
        SceneManager.LoadScene("Main");
    }

    public void Sair()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
