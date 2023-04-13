using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomePageControl : MonoBehaviour
{
    public GameObject ExitPanel;

    public void Start()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }
    public void PlayBtn()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        ExitPanel.SetActive(true);
      
    }

    public void Answer(string answer)
    {
        
        if(answer == "Yes")
        {
            Application.Quit(); 
        }
        else
        {
            ExitPanel.SetActive(false);
        }
       
        
    }
}
