using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomePageControl : MonoBehaviour
{
    public GameObject ExitPanel;

    public void Start()
    {
        // If the time scale is set to 0 (paused), set it to 1 (normal time)
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }

    public void PlayBtn()
    {
        // Load scene with build index 1 (assumes the scene exists)
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        // Activate the exit panel
        ExitPanel.SetActive(true);
    }

    public void Answer(string answer)
    {
        if (answer == "Yes")
        {
            // Quit the application
            Application.Quit();
        }
        else
        {
            // Deactivate the exit panel
            ExitPanel.SetActive(false);
        }
    }

}
