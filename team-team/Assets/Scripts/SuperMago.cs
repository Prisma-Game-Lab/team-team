using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FMODUnity;

public class SuperMago : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerEffects playerEffects;
    private Throw _throw;
    private GameObject[] potionVariants;

    public bool full = false;
    public PotionEffect potionType;
    public int playerIndex;

    private StudioEventEmitter soundscape;
    private float paramValue; //valor do evento

    // Start is called before the first frame update
    void Start()
    {
        playerInput = this.GetComponent<PlayerInput>();
        playerEffects = this.GetComponent<PlayerEffects>();
        _throw = this.GetComponent<Throw>();
        potionVariants = GameController.Instance.potionVariants;
        soundscape = null;
        GameObject go = GameObject.Find("trilha");
        paramValue = 1.0f;
        if(go != null)
        {
            soundscape = go.GetComponent<StudioEventEmitter>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(paramValue == 0.0f && soundscape != null)
        {
            soundscape.SetParameter("SupermegaMago", paramValue);
        }
        
        if (InputManager.GetKeyDown(playerInput.controllerScheme, "Action2"))
        {
            if(_throw.barFill.fillAmount == 1.0f)
            {
                full = true;
                //this.gameObject.transform.GetChild(0).gameObject.GetComponent<Collider>().enabled = false;
                //this.GetComponent<Rigidbody>().useGravity = false;
                Debug.Log("Supermago iniciado");
                paramValue = 0.0f; //liga baixo
                
                this.transform.GetChild(8).gameObject.SetActive(true);
            }                      
        }
        if(full)
        {
            _throw.barFill.fillAmount -= 0.2f * Time.deltaTime;
            if (_throw.barFill.fillAmount == 0.0f)
            {
                Debug.Log("Fim suoermago");
                paramValue = 1.0f; //desliga baixo
                if(soundscape != null) soundscape.SetParameter("SupermegaMago", 1.0f);
                full = false;
                this.transform.GetChild(8).gameObject.SetActive(false);
                // this.gameObject.transform.GetChild(0).gameObject.GetComponent<Collider>().enabled = true;
                //this.GetComponent<Rigidbody>().useGravity = true;
            }               
        }      
    }

    private void HitMago(PlayerEffects player)
    {
        if(full)
        {
            int ChosenPotionEffect = Random.Range(0, System.Enum.GetValues(typeof(PotionEffect)).Length);//, potionVariants.Length);
            potionType = (PotionEffect) ChosenPotionEffect;
            player.AddEffect(potionType, 3);
            GameController.Instance.AddPoints(25, playerIndex);
        }        
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerEffects pe = other.gameObject.GetComponent<PlayerEffects>();       

        if(other.gameObject.CompareTag("Player"))
        {
            this.HitMago(pe);
        }

        if(other.gameObject.CompareTag("Wall"))
        {
            StartCoroutine(DisableCollider(other, 5));
        }
    }
    IEnumerator DisableCollider (Collider other, float duration)
    {
        if(full)
        {
            if (other.gameObject.CompareTag("Wall"))
            {
                other.enabled = false;
            }
            yield return new WaitForSecondsRealtime(1);

            other.enabled = true;
        }
    }

    // //K: seta parametro do FMOD enquanto super mago estiver acionado
    // IEnumerator SetFMODParameter (float duration)
    // {
    //     if(soundscape == null) yield break;
    //     float inc = 0.0f;
    //     float step = 0.3f;
    //     while(inc < duration)
    //     {
    //         soundscape.SetParameter("SupermegaMago", 1.0f);
    //         yield return new WaitForSecondsRealtime(step);
    //         inc += step;
    //     }
    //     soundscape.SetParameter("SupermegaMago", 0.0f);
    // }
}
