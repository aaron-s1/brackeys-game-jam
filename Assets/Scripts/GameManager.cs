using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // public static GameObject player { get; private set; }

    public bool textLoadNext;
    int currentLevel;

    void Awake() {
        // if (player == null)
            // player = GameObject.FindGameObjectWithTag("Player");

        currentLevel = SceneManager.GetActiveScene().buildIndex + 1;
    }

    void Update()
    {
        if (textLoadNext)
        {
            textLoadNext = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                // currentLevel++);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
