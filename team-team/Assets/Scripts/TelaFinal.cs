using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TelaFinal : MonoBehaviour
{
    private GameObject[] characterPanels;
    int playersQtd;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    void Initialize()
    {
         if(PersistentInfo.Instance == null)
        {
            playersQtd = 4;
        }
        else
        {
            playersQtd = PersistentInfo.Instance.playersQtd;
        }


        GameObject parent = transform.Find("PainelPersonagens").gameObject;
        int childCount = parent.transform.childCount;
        characterPanels = new GameObject[childCount];
        for(int i = 0; i < childCount; i++)
        {
            characterPanels[i] =  parent.transform.GetChild(i).gameObject;
            characterPanels[i].SetActive(i < playersQtd); //disabla painel se player não está no jogo
        }
    }
    
    //função para ser chamada quando o jogo termina e a telafinal deva ser acionada
    public void ActivateTelaFinal()
    {
        Initialize();
        this.gameObject.SetActive(true);
        //ativa ou desativa paineis de acordo com o numero de jogadores jogando
        for(int i = 0; i < 4; i++)
        {
            characterPanels[i].SetActive(playersQtd > i);
        }
        for(int i = 0; i < playersQtd; i++)
        {
            characterPanels[i].GetComponentInChildren<Text>().text = GameController.Instance.teamPoints[i].ToString();
        }

        //ordena e muda ordem?

        //indica quem ganhou?

        //toca som?

        // muda fotinhos pra ficar certo com os players?
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MenuTemp");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Update is called once per frame
    void Update()
    {
        

    }
}
