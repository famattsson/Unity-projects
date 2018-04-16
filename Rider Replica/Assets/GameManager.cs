using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    GameObject player;
    bool gameHasEnded = false;
    public GameObject GameOverMenu;
    public Transform spawnPoint;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        if ((player.transform.position.y <= -20 && !gameHasEnded) || 
            (Vector3.Dot(player.transform.up, Vector3.down) > 0.3 && !gameHasEnded && CarController.isGrounded))
        {
            gameHasEnded = true;
            GameOver();
        }
	}

    public void GameOver()
    {
        Time.timeScale = 0f;
        GameOverMenu.SetActive(true);
    }
    public void Retry ()
    {
        CarController.isGrounded = false;
        gameHasEnded = false;
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        rb.rotation = 0f;
        rb.angularVelocity = 0f;
        rb.velocity = new Vector2(0f,0f);
        Time.timeScale = 1f;
        GameOverMenu.SetActive(false);
        player.transform.position = spawnPoint.position;
    }
    public void Quit ()
    {
        Application.Quit();
    }
}
