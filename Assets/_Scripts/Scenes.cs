using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour
{
    public GameObject PausePanel;


    public void PauseButtonPressed()
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ContinueButtonPressed()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ChangeScenes(int numberScenes)
    {
        SceneManager.LoadScene(numberScenes);
    }

    public void Exit()
    {
        Application.Quit(); 
    }

    public void DeleteProgress()
    {
        PlayerPrefs.DeleteAll();
    }

    public void timeScaler()
    {
        Time.timeScale = 1;
    }

    public void StartTheCutscene()
    {
        CutsceneManager.Instance.StartCutscene("Cutscene");
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseButtonPressed();
        }

    }
}
