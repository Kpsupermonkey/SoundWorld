using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuManager : MonoBehaviour
{

    public GameObject SettingsMenu;
   
    public void PlayGame()
    {
        //This will load the scene that we have chosen once we press the Start button
        SceneManager.LoadScene("SampleScene");
    }
    

    public void QuitGame()
    {
        // Once we click on the button this will exit the application
        Application.Quit();
    }

    public void Credits()
    {
        //This will load the scene that we have chosen once we press the Credits button
        SceneManager.LoadScene("CreditsMenu");
    }


    public void OpenSettingsMenuFromMain()
    {
        PlayerPrefs.SetString("SettingsOrigin", "MainMenu"); // Store origin
        SettingsMenu.SetActive(true);
    }
    
}