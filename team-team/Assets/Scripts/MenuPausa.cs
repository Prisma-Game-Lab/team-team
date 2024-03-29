﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    public static bool pausado = false;

    public GameObject pauseMenuUi;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Joystick1Button7) || Input.GetKeyDown(KeyCode.Joystick1Button9))//||Input.GetKeyDown("Pause Keyboard"))
        {
            if (pausado)
            {
                Continuar();
            }
            else
            {
                Pausar();
            }
        }

    }

    public void Continuar()
    {
        pauseMenuUi.SetActive(false);
        Time.timeScale = 1.0f;
        pausado = false;

    }

    void Pausar()
    {
        pauseMenuUi.SetActive(true);
        Time.timeScale = 0f;
        pausado = true;

    }

    public void MenuPrincipal()
    {
        SceneManager.LoadScene("Menu");
    }
}
