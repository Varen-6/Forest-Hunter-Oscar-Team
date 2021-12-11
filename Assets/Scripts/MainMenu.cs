using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject BunnyDrop;
    public GameObject WolfDrop;
    public GameObject DeerDrop;
    
    public void ExitButton()
    {
        Application.Quit();
        Debug.Log("GameClosed");
    }

    public void ApplySettings()
    {
        GameSettings.BunnyCount = Convert.ToInt32(BunnyDrop.GetComponent<Dropdown>().options[BunnyDrop.GetComponent<Dropdown>().value].text);
        GameSettings.WolfCount = Convert.ToInt32(WolfDrop.GetComponent<Dropdown>().options[WolfDrop.GetComponent<Dropdown>().value].text);
        GameSettings.DeerCount = Convert.ToInt32(DeerDrop.GetComponent<Dropdown>().options[DeerDrop.GetComponent<Dropdown>().value].text);
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
}
