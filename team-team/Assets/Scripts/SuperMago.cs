using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperMago : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerEffects playerEffects;
    private Throw _throw;
    private GameObject[] potionVariants;
    public bool full = false;
    public PotionEffect potionType;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = this.GetComponent<PlayerInput>();
        playerEffects = this.GetComponent<PlayerEffects>();
        _throw = this.GetComponent<Throw>();
        potionVariants = GameController.Instance.potionVariants;
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.GetKeyDown(playerInput.controllerScheme, "Action2"))
        {
            if(_throw.barFill.fillAmount == 1.0f)
            {
                full = true;
                this.gameObject.transform.GetChild(0).gameObject.GetComponent<Collider>().enabled = false;
                this.GetComponent<Rigidbody>().useGravity = false;
                Debug.Log("Supermago iniciado");
            }                      
        }
        if(full)
        {
            _throw.barFill.fillAmount -= 0.2f * Time.deltaTime;
            if (_throw.barFill.fillAmount == 0.0f)
            {
                Debug.Log("Fim suoermago");
                full = false;
                this.gameObject.transform.GetChild(0).gameObject.GetComponent<Collider>().enabled = true;
                this.GetComponent<Rigidbody>().useGravity = true;
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
        }        
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerEffects pe = other.gameObject.GetComponent<PlayerEffects>();
        this.HitMago(pe);
    }
}
