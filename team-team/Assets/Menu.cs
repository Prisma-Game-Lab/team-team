using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private CharSelection charSelection;
    public void Iniciar()
    {
            SceneManager.LoadScene("Main");
    }

    public void Sair()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    //K: temp para hacktudo
    public void IniciarTemp(int playersQtd)
    {
        PersistentInfo.Instance.playersQtd = playersQtd;
        SceneManager.LoadScene("CenaGDFinal");
    }
}
