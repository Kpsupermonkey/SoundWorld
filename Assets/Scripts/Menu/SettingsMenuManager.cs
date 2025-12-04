using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour
{
    public GameObject ParentMenu;
    public GameObject SettingsPage;
    public Button BackButton;

    private int currentPageIndex = 0;
    private GameObject[] pages;
    private string previousMenu;

    private void Start()
    {
        // Store the pages in an array for easy navigation
            pages = new GameObject[] { SettingsPage};

            // Ensure only the first page is visible at the start
            //UpdatePageVisibility();

            // Store the previous menu (Pause Menu or Main Menu)
            previousMenu = SceneManager.GetActiveScene().name;
        
    }

    public void NextPage()
    {
        if (currentPageIndex < pages.Length - 1)
        {
            currentPageIndex++;
            UpdatePageVisibility();
        }
    }

    public void BackPage()
    {
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            UpdatePageVisibility();
        }
        else
        {
            GoBack();
        }
    }

    private void UpdatePageVisibility()
    {
        if (pages == null || pages.Length == 0)
        {
            Debug.LogError("Pages array is not initialized properly!");
            return;
        }

        for (int i = 0; i < pages.Length; i++)
        {
            if (pages[i] == null)
            {
                Debug.LogError($"Page at index {i} is null!");
                continue;
            }
            pages[i].SetActive(i == currentPageIndex);
        }
    }

    public void GoBack()
    {
        // Check if the current scene is NOT the "SettingsMenu" scene.
        if (SceneManager.GetActiveScene().name != "SettingsMenu")
        {
            ParentMenu.SetActive(true);

            // Hide the settings menu by disabling its parent GameObject.
            SettingsPage.gameObject.SetActive(false);
        }
        else
        {
            // If the current scene is "SettingsMenu," return to the main menu.
            SceneManager.LoadScene("Title Menu");
        }
    }
} 

                       