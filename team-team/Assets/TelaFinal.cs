using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TelaFinal : MonoBehaviour
{
    private GameObject[] characterPanels;
    // Start is called before the first frame update
    void Start()
    {
        int playersQtd = 4;//PersistentInfo.Instance.playersQtd;

        GameObject parent = transform.Find("PainelPersonagens").gameObject;
        int childCount = parent.transform.childCount;
        characterPanels = new GameObject[childCount];
        for(int i = 0; i < childCount; i++)
        {
            characterPanels[i] =  parent.transform.GetChild(i).gameObject;
            characterPanels[i].SetActive(i < playersQtd); //disabla painel se player não está no jogo
        }
        this.gameObject.SetActive(false);
    }

    //função para ser chamada quando o jogo termina e a telafinal deva ser acionada
    public void ActivateTelaFinal()
    {
        this.gameObject.SetActive(true);
        int playersQtd = 4;//PersistentInfo.Instance.playersQtd;
        //ativa ou desativa paineis de acordo com o numero de jogadores jogando
        for(int i = 0; i < playersQtd; i++)
        {
            characterPanels[i].GetComponentInChildren<Text>().text = GameController.Instance.teamPoints[i].ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        

    }
}
