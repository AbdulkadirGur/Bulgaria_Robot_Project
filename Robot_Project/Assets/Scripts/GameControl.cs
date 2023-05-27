using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;



public class GameControl : MonoBehaviour
{
    // General Settings
    public int levelSucces; // The required number of successful choices to win the level
    int currentSucces; // The current number of successful choices
    int numChoices; // The total number of choices available
                    //-----------------------
    GameObject chooseButton; // Reference to the choose button object
    GameObject itselfBTN; // Reference to the itself button object
                          //-----------------------
    public AudioSource[] voices; // Array of audio sources for playing sounds
    public GameObject[] buttons; // Array of button objects
    public Sprite defaultSprite; // Default sprite for buttons
    public TextMeshProUGUI Count; // Text object for displaying count
    public GameObject[] GameOverPanels; // Array of game over panel objects
    public UnityEngine.UI.Slider TimeSlider; // Slider object for time display
                                             //-----------------------
                                             // Count    
    public float Alltime; // Total time allowed
    float minute; // Minutes portion of the time
    float second; // Seconds portion of the time
    bool Timer; // Flag for enabling the timer
    float lastTime; // Last recorded time
                    //-----------------------

    public GameObject grid; // Grid object
    public GameObject gridText; // Grid text object
    public GameObject LevelPool; // Level pool object
    bool createState; // Flag for creating objects
    int createCount; // Counter for created objects
    int allImageNum; // Total number of images





    void Start()
    {
        numChoices = 0; // Initialize the number of choices to 0
        Timer = true; // Enable the timer
        lastTime = 0; // Set the last recorded time to 0
        TimeSlider.value = lastTime; // Set the initial value of the time slider to the last recorded time
        TimeSlider.maxValue = Alltime; // Set the maximum value of the time slider to the total time allowed
        createState = true; // Enable the state for creating objects
        createCount = 0; // Set the create count to 0
        allImageNum = LevelPool.transform.childCount; // Get the total number of images in the level pool
        levelSucces = (LevelPool.transform.childCount) / 2; // Calculate the required number of successful choices based on the number of images
        currentSucces = 0; // Initialize the current number of successful choices to 0

        StartCoroutine(Create()); // Start the coroutine for creating objects
    }


    void Update()
    {
        if (Timer && lastTime != Alltime)
        {
            lastTime += Time.deltaTime; // Increase the last recorded time by the time passed since the last frame
            TimeSlider.value = lastTime; // Update the value of the time slider with the last recorded time

            if (TimeSlider.maxValue == TimeSlider.value)
            {
                Timer = false; // Disable the timer
                GameOver(); // Call the GameOver function
            }
        }
    }


    IEnumerator Create()
    {
        while (createState)
        {
            int rnum = Random.Range(0, LevelPool.transform.childCount - 1); // Generate a random number within the range of available child objects in the LevelPool
            if (LevelPool.transform.GetChild(rnum).gameObject != null)
            {
                LevelPool.transform.GetChild(rnum).transform.SetParent(grid.transform); // Set the parent of the randomly selected child object to the grid object
                createCount++; // Increase the create count

                if (createCount == allImageNum)
                {
                    createState = false; // Disable the state for creating objects
                    Destroy(LevelPool.gameObject); // Destroy the LevelPool object
                }
            }
        }

        yield return new WaitForSeconds(0.1f); // Wait for a short duration before executing the next iteration
    }



    void GameOver()
    {
        GameOverPanels[0].SetActive(true); // Activate the first game over panel
    }

    void Win()
    {
        GameOverPanels[1].SetActive(true); // Activate the second game over panel
    }

    public void HomeMenu()
    {
        SceneManager.LoadScene("HomePage"); // Load the "HomePage" scene
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
    }

    public void Pause()
    {
        GameOverPanels[2].SetActive(true); // Activate the pause panel
        Time.timeScale = 0; // Pause the game by setting the time scale to 0
    }

    public void Continue()
    {
        GameOverPanels[2].SetActive(false); // Deactivate the pause panel
        Time.timeScale = 1; // Resume the game by setting the time scale to 1
    }

    public void NextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            SceneManager.LoadScene("HomePage"); // If the current scene is the third level, load the "HomePage" scene
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Load the next scene in the build index
        }
    }


    public void buttonsState(bool state)
    {
        foreach (var item in buttons)
        {
            if (item != null)
            {
                item.GetComponent<UnityEngine.UI.Image>().raycastTarget = state;
                // Access the UnityEngine.UI.Image component of each button (item) and set its raycastTarget property to the specified state.
                // This controls the clickability of the buttons.
            }
        }
    }

    public void GetObjects(GameObject obj)
    {
        itselfBTN = obj;  // Assign the input object to the itselfBTN variable, representing the button object.
        itselfBTN.GetComponent<UnityEngine.UI.Image>().sprite = itselfBTN.GetComponentInChildren<SpriteRenderer>().sprite;  // Set the sprite of the
         // UnityEngine.UI.Image component of itselfBTN to the sprite of the SpriteRenderer component that is a child of itselfBTN. This ensures that the button
         // displays the appropriate sprite visually.
        itselfBTN.GetComponent<UnityEngine.UI.Image>().raycastTarget = false;  // Set the raycastTarget property of the UnityEngine.UI.Image component of itselfBTN
                                                                               // to false, making the button non-clickable.
    }

    public void MyButtonClick(int value)
    {
        // Call the Control method and pass the value as an argument
        Control(value);
    }


    void Control(int getValue)
    {
        if (numChoices == 0)
        {
            // If there are no previous choices, assign the current value and button
            numChoices = getValue;
            chooseButton = itselfBTN;
        }
        else
        {
            // If there is a previous choice, start a coroutine to check the current and previous choices
            StartCoroutine(checkIT(getValue));
        }
    }


    IEnumerator checkIT(int getValue)
    {
        // Disable all buttons temporarily
        buttonsState(false);
        yield return new WaitForSeconds(1);

        if (numChoices == getValue)
        {
            // If the current choice matches the previous choice
            currentSucces++;
            // Disable the images of the matched buttons
            chooseButton.GetComponent<UnityEngine.UI.Image>().enabled = false;
            itselfBTN.GetComponent<UnityEngine.UI.Image>().enabled = false;
            // Play a sound effect
            voices[0].Play();
            // Show the grid text
            gridText.SetActive(true);

            // Reset the choices and enable the buttons again
            numChoices = 0;
            chooseButton = null;
            buttonsState(true);

            if (currentSucces == levelSucces)
            {
                // If all matches are found, trigger the Win condition
                Win();
            }
        }
        else
        {
            // If the current choice does not match the previous choice
            voices[1].Play();
            // Reset the sprites of the unmatched buttons to the default sprite
            itselfBTN.GetComponent<UnityEngine.UI.Image>().sprite = defaultSprite;
            chooseButton.GetComponent<UnityEngine.UI.Image>().sprite = defaultSprite;

            // Enable the buttons again
            buttonsState(true);
            // Reset the choices
            numChoices = 0;
            chooseButton = null;
            // Show the grid text
            gridText.SetActive(true);
        }
    }
    public void NumberBtnClick()
    {
        // Hide the grid text
        gridText.SetActive(false);
    }

}
