using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Localisation : MonoBehaviour
{
    public int language;
    [SerializeField] private int SceneNumber;

    void Start()
    {
        language = PlayerPrefs.GetInt("language", language);
    }

    public void RussianLanguage()
    {
        language = 0;
        PlayerPrefs.SetInt("language", language);
        SceneManager.LoadScene(SceneNumber);
    }

    public void EnglishLanguage()
    {
        language = 1;
        PlayerPrefs.SetInt("language", language);
        SceneManager.LoadScene(SceneNumber);
    }

    public void TatarLanguage()
    {
        language = 2;
        PlayerPrefs.SetInt("language", language);
        SceneManager.LoadScene(SceneNumber);
    }

    void Update()
    {
        
    }
}
