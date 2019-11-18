using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public CharSelection charSelection;
    public string[] availableLevels;
    private int playerCount;
    public void Iniciar()
    {
        SceneManager.LoadScene("Main");
    }

    public void Sair()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    //J: Armazena numero de jogadores selecionado
    public void setPlayerCount(int newCount)
    {
        playerCount = newCount;
    }

    //J: Inicia cena selecionada com numero de jogadores
    public void IniciarTemp(int selectedLevel)
    {
        PersistentInfo.Instance.playersQtd = playerCount;
        SceneManager.LoadScene(availableLevels[selectedLevel]);
    }
    public void StartGame(int selectedLevel)
    {

        if (charSelection.CheckIfPlayersReady())
        {
            PersistentInfo.Instance.PlayerData = charSelection.PlayerData;
            PersistentInfo.Instance.playersQtd = charSelection.numPlayers;
            SceneManager.LoadScene(availableLevels[selectedLevel]);
        }
    }
}
