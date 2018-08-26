using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

class WinCondition
{
    protected float goal;
    protected float counter;
    protected GameManager gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    protected bool isPaused;

    public WinCondition (float goal, GameManager gameManager, float counter = 0)
    {
        this.goal = goal;
        this.counter = counter;
        this.gameManager = gameManager;
    }
    public virtual void ResetCondition()
    {
        isPaused = false;
        counter = 0;
    }
    public virtual void PauseCondition()
    {
        isPaused = true;
    }
    public virtual void ResumeCondition()
    {
        isPaused = false;
    }
    public virtual void UpdateCondition() {
        if (counter >= goal && !isPaused)
        {
            gameManager.Win();
        }
    }
}

class Exterminate : WinCondition
{
    public Exterminate (float goal, GameManager gameManager, float counter = 0) : base(goal,gameManager,counter) { }
    public override void UpdateCondition()
    {
        gameManager.ConditionText.text = "Hostiles eliminated";
        gameManager.GoalText.text = counter + "/" + goal;
        counter = Mathf.Clamp(gameManager.GetKilledEnemies(), 0, goal);
        base.UpdateCondition();
    }
}

class TimeTrial : WinCondition
{
    public TimeTrial(float goal,  GameManager gameManager, float counter = 0) : base(goal, gameManager, counter) {}
    public override void UpdateCondition()
    {
        if(Time.timeScale != 0)
        {
            counter = Mathf.Clamp(counter + Time.deltaTime,0,goal);
            gameManager.ConditionText.text = "Time survived";
            gameManager.GoalText.text = Math.Round(counter,0) + "/" + goal;
            base.UpdateCondition();
        }
    }
}

public class GameManager : MonoBehaviour {

    public int score;
    public Text ConditionText;
    public Text GoalText;
    private int KilledEnemies;
    public Animator animator;
    public Text pauseMenuText;
    public Text gameOverMenuText;
    public GameObject PauseMenu;
    private GameObject player;
    private GameObject Base;
    private WinCondition winCondition;
    private Dictionary<int, WinCondition> winConditions;
    private bool hasWon;
    private bool hasLost;
    public GameObject OptionsMenu;
    public GameObject gameOverMenu;

    public int GetKilledEnemies ()
    {
        return KilledEnemies;
    }

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Base = GameObject.FindGameObjectWithTag("Base");
        score = 0;
        winConditions = new Dictionary<int, WinCondition>() {
            {0, new Exterminate(20, this, 0)}, {1, new TimeTrial(90, this, 0)}
        };
        KilledEnemies = 0;
        RandomizeLevel();
    }

    public void RandomizeLevel()
    {
        winCondition = winConditions[UnityEngine.Random.Range(0, winConditions.Count)];
        if(hasLost || hasWon)
        {
            Restart();
        }
    }

    public void Options ()
    {
        if(PauseMenu.activeSelf)
        {
            PauseMenu.SetActive(false);
            OptionsMenu.SetActive(true);
        }
        else
        {
            PauseMenu.SetActive(true);
            OptionsMenu.SetActive(false);
        }
    }

    public void Update()
    {
        winCondition.UpdateCondition();

        if (Input.GetButtonDown("Cancel") && !PauseMenu.activeSelf)
        {
            Pause();
        }
        else if (Input.GetButtonDown("Cancel") && PauseMenu.activeSelf && !hasWon && !hasLost)
        {
            Resume();
        }
    }

    public void GameOverMenu (string message)
    {
        Debug.Log("GameOverMenu");
        gameOverMenu.SetActive(true);
        gameOverMenuText.text = message;
        Time.timeScale = 0;
        player.GetComponent<Weapon>().enabled = false;
    }

    public void GameOver(string reason = "")
    {
        if(!hasLost)
        {
            hasLost = true;
            GameOverMenu("Game Over!");
        }
    }

    public void Restart()
    {  
        Resume();
        Trigger("Crossfade");
    }

    public void Trigger(string name)
    {
        animator.SetTrigger(name);
    }
    
    public void UpdateKilledEnemies()
    {
        KilledEnemies++;
    }

    public void Win()
    {
        if (!hasWon)
        {
            hasWon = true;
            player.GetComponent<PlayerStats>().TriggerAnim("Win");
        }
    }



    public void ResetLevel()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.GetComponent<Enemy>().Die(false);
        }
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerStats>().Reset();
        Base.GetComponent<Base>().Reset();
        KilledEnemies = 0;
        winCondition.ResetCondition();
        hasLost = false;
        hasWon = false;
        foreach (GameObject bullet in GameObject.FindGameObjectsWithTag("Bullet"))
        {
            Destroy(bullet);
        }
    }

    public void Resume()
    {
        gameOverMenu.SetActive(false);
        PauseMenu.SetActive(false);
        Time.timeScale = 1;
        pauseMenuText.text = "Paused";
        player.GetComponent<Weapon>().enabled = true;
    }

    public void Pause(string message = "Paused")
    {
        if(OptionsMenu.activeSelf)
        {
            OptionsMenu.SetActive(false);
        }
        PauseMenu.SetActive(true);
        pauseMenuText.text = message;
        Time.timeScale = 0;
        player.GetComponent<Weapon>().enabled = false;
    }

    public void PauseWinCondition()
    {
        winCondition.PauseCondition();
    }
    public void ResumeWinCondition()
    {
        winCondition.ResumeCondition();
    }
}
