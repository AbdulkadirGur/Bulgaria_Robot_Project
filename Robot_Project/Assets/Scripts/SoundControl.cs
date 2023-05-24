using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundControl : MonoBehaviour
{
    private static GameObject instance;

    private void Awake()
    {
        // Don't destroy this game object when a new scene is loaded
        DontDestroyOnLoad(gameObject);

        // Check if an instance of this game object already exists
        if (instance == null)
        {
            // If not, set this instance as the instance
            instance = gameObject;
        }
        else
        {
            // If an instance already exists, destroy this game object
            Destroy(gameObject);
        }
    }

    


}
