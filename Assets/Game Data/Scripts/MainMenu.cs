using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string gamePlaySceneName;
    public void PlayClick()
    {
        SceneManager.LoadScene(gamePlaySceneName);
    }
}
