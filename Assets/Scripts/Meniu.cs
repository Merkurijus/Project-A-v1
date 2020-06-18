using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Meniu : MonoBehaviour
{
    public GameObject sustabdytaUI;
    private GameMaster gameMaster;
    
    private void Start()
    {
        
        gameMaster = FindObjectOfType<GameMaster>();
    }
    public void Pause()
    {
        Time.timeScale = 0;
        sustabdytaUI.SetActive(true);
       
    }
    public void Resume()
    {
        Time.timeScale = 1f;
        sustabdytaUI.SetActive(false);
       
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Zaisti()
    {
        SceneManager.LoadScene(2);
    }
    public void MainMeniu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);

    }
}
