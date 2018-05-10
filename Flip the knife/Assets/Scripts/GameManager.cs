using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public GameObject gameOverMenu;
    public GameObject knife;

    public void GameOver ()
    {
        knife.GetComponent<Knife>().enabled = false;
        knife.GetComponent<AudioSource>().Stop();
        gameOverMenu.SetActive(true);
        
    }

    public void Retry()
    {
        Debug.Log("Retried");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
