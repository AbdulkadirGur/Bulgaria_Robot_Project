using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;



public class GameControl : MonoBehaviour
{
    //General Settings
    public int levelSucces;
    int currentSucces;
    int numChoices;
    //-----------------------
    GameObject chooseButton;
    GameObject itselfBTN;
    //-----------------------
    public AudioSource [] voices;
    public GameObject[] buttons;
    public Sprite defaultSprite;
    public TextMeshProUGUI Count;
    public GameObject[] GameOverPanels ;
    public UnityEngine.UI.Slider TimeSlider;
    //-----------------------
    //Count    
    public float Alltime;
    float minute;
    float second;
    bool Timer;
    float lastTime;
    //-----------------------

    public GameObject grid;
    public GameObject gridText;
    public GameObject LevelPool;
    bool createState;
    int createCount;
    int allImageNum;


    void Start()
    {
        numChoices = 0;
        Timer= true;
        lastTime = 0;
        TimeSlider.value = lastTime;
        TimeSlider.maxValue = Alltime;
        createState= true;
        createCount = 0;
        allImageNum = LevelPool.transform.childCount;
        levelSucces = (LevelPool.transform.childCount )/2;
        
        currentSucces = 0;

        StartCoroutine(Create());



    }

    void Update()
    {

        if(Timer  && lastTime!=Alltime)
        {
            lastTime += Time.deltaTime;
            TimeSlider.value = lastTime;   

            if(TimeSlider.maxValue == TimeSlider.value)
            {
                Timer = false;               
                GameOver();
            }
            /*minute = Mathf.FloorToInt(Alltime / 60);
            second = Mathf.FloorToInt(Alltime % 60);

            // Count.text = Mathf.FloorToInt(Alltime).ToString();

            Count.text = string.Format("{0:00}:{1:00}", minute, second);*/
        }
        
       
    }

    IEnumerator Create()
    {
        while (createState)
        {
            int rnum = Random.Range(0, LevelPool.transform.childCount - 1);
            if(LevelPool.transform.GetChild(rnum).gameObject != null)
            {
                LevelPool.transform.GetChild(rnum).transform.SetParent(grid.transform);
                createCount++;

                if (createCount == allImageNum)
                {
                    createState= false;
                    Destroy(LevelPool.gameObject);
                }
            }
            
            
        }

        yield return new  WaitForSeconds(.1f);
    }


    void GameOver()
    {
        GameOverPanels[0].SetActive(true);
    }

    void Win()
    {
        GameOverPanels[1].SetActive(true);
    }

    public void HomeMenu()
    {
        SceneManager.LoadScene("HomePage");

    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Pause()
    {
        GameOverPanels[2].SetActive(true);
        Time.timeScale = 0;
    }

    public void Continue()
    {
        GameOverPanels[2].SetActive(false);
        Time.timeScale = 1;
        
    }
    int sayac = 1;
    public void NextLevel()
    {


       
        if (sayac > 3)
        {
            SceneManager.LoadScene(0);
        }
        sayac++;
        SceneManager.LoadScene(sayac);
    }

    public void buttonsState(bool state)  // coklu tiklamaya izin vermiyor tum butonlari kapatiyor
    {
        foreach (var item in buttons)
        {
            if (item !=null)    // dizide silinen objeleri dikkate almiyor
            {
                item.GetComponent<UnityEngine.UI.Image>().raycastTarget = state;
            }
           
        }
    }
    public void GetObjects(GameObject obj) // tiklandiginda objeyi esitliyor ve childin imageni atiyor
    {
        itselfBTN=obj;
        itselfBTN.GetComponent<UnityEngine.UI.Image>().sprite = itselfBTN.GetComponentInChildren<SpriteRenderer>().sprite;
        itselfBTN.GetComponent<UnityEngine.UI.Image>().raycastTarget = false;
    }
    public void MyButtonClick(int value)
    {
        Control(value);
    }

    void Control(int getValue )
    {
        if (numChoices == 0)// first button click 
        {
            numChoices = getValue;
            chooseButton= itselfBTN;
            
        }
        else // Pressing any button a second time
        {
           StartCoroutine(checkIT(getValue));
        }
    }

    IEnumerator checkIT(int getValue) // butonlara tiklandiginda 1 saniye gec calistiryor 
    {
        buttonsState(false);
        yield return new WaitForSeconds(1);
        if (numChoices == getValue)
        {
            //Debug.Log("Matched");
            currentSucces++;
            chooseButton.GetComponent<UnityEngine.UI.Image>().enabled = false;
            itselfBTN.GetComponent<UnityEngine.UI.Image>().enabled = false;
            voices[0].Play();
            gridText.SetActive(true);
            

            //itselfBTN.GetComponent<UnityEngine.UI.Image>().enabled = false;
            // itselfBTN.GetComponent<UnityEngine.UI.Button>().enabled = false;
            /*
            Destroy(chooseButton.gameObject);
            Destroy(itselfBTN.gameObject);*/

            numChoices = 0;
            chooseButton = null;
            buttonsState(true);

            if(currentSucces==levelSucces)
            {
               
                Win();
            }

        }
        else
        {
            // Debug.Log("did not Matched");
            voices[1].Play();
            itselfBTN.GetComponent<UnityEngine.UI.Image>().sprite = defaultSprite;
            chooseButton.GetComponent<UnityEngine.UI.Image>().sprite = defaultSprite;
           
            buttonsState(true);
            numChoices = 0;
            chooseButton = null;
            gridText.SetActive(true);
        }
    }

    public void NumberBtnClick()
    {
        gridText.SetActive(false); ;
    }
}
