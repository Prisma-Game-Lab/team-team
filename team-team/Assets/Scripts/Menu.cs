using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public CharSelection charSelection;
    public string[] availableLevels;
    private int playerCount;

    public void Sair()
    {
        Debug.Log("Quit");
        Application.Quit();
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
