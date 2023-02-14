using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool textLoadNext;
    int currentLevel;

    void Awake() {
        currentLevel = SceneManager.GetActiveScene().buildIndex + 1;
    }

    void Update()
    {
        if (textLoadNext)
        {
            textLoadNext = false;
            SceneManager.LoadScene(currentLevel++);
        }
    }
}
