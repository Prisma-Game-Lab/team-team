using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperMago : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerEffects playerEffects;
    private Throw _throw;
    public bool full = false;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = this.GetComponent<PlayerInput>();
        playerEffects = this.GetComponent<PlayerEffects>();
        _throw = this.GetComponent<Throw>();
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.GetKeyDown(playerInput.controllerScheme, "Action2"))
        {
            if(_throw.barFill.fillAmount == 1.0f)
            {
                full = true;
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
            }               
        }      
    }
}
