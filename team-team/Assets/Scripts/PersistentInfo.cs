using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//classe usada para passar informações de uma cena para outra
public class PersistentInfo : MonoBehaviour
{
    
    public int playersQtd;

    //padrão singleton
    public static PersistentInfo Instance {get; private set;}
    // Start is called before the first frame update
    void Awake()
    {
        if(Instance != null)
        {
            GameObject.Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            Object.DontDestroyOnLoad(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
