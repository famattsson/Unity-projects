using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Pathfinding;
using UnityEngine.SceneManagement;
using TMPro;

class WinCondition
{
    protected float goal;
    protected float counter;
    protected GameManager gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    protected bool isPaused;
    protected Text conditionText;
    protected Text counterText;
    public bool achieved = false;

    public WinCondition(float goal, GameManager gameManager, float counter = 0)
    {
        this.goal = goal;
        this.counter = counter;
        this.gameManager = gameManager;
    }
    public void AssignIndicator (GameObject indicator)
    {
        indicator.SetActive(true);
        foreach (Transform textElem in indicator.transform)
        {
            if(textElem.name.Contains("Text"))
            {
                conditionText = textElem.GetComponent<Text>();
            }
            if(textElem.name.Contains("Counter"))
            {
                counterText = textElem.GetComponent<Text>();
            }   
        }
    }
    public void DivideGoalBy(float value)
    {
        goal /= value;
    }
    public void MultiplyGoalBy(float value)
    {
        Debug.Log(value);
        goal *= value;
    }
    public virtual void ResetCondition()
    {
        isPaused = false;
        achieved = false;
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
            achieved = true;
            gameManager.Win();
        }
    }
}

class Exterminate : WinCondition
{
    public Exterminate(float goal, GameManager gameManager, float counter = 0) : base(goal, gameManager, counter) { }
    public override void UpdateCondition()
    {
        conditionText.text = "Hostiles eliminated";
        counterText.text = counter + "/" + goal;
        counter = Mathf.Clamp(gameManager.GetKilledEnemies(), 0, goal);
        base.UpdateCondition();
    }
}

class TimeTrial : WinCondition
{
    public TimeTrial(float goal, GameManager gameManager, float counter = 0) : base(goal, gameManager, counter) { }
    public override void UpdateCondition()
    {
        if(Time.timeScale != 0)
        {
            counter = Mathf.Clamp(counter + Time.deltaTime,0,goal);
            conditionText.text = "Time survived";
            counterText.text = Math.Round(counter,0) + "/" + goal;
            base.UpdateCondition();
        }
    }
}

public class GameManager : MonoBehaviour {

    public int score;
    public GameObject[] objectiveIndicators;
    public Text ConditionText;
    public Text GoalText;
    private int KilledEnemies;
    public Animator animator;
    public GameObject PauseMenu;
    private GameObject planetBase;
    private List<WinCondition> activeWinConditions;
    private List<WinCondition> possibleWinConditions;
    private bool hasWon;
    private bool hasLost;
    public GameObject OptionsMenu;
    public GameObject gameOverMenu;
    public GameObject galaxyMap;
    public GameObject player;
    public GameObject winMenu;
    bool canPause = true;
    private PlanetButton planet;
    private float ScanDelay = 2;

    public PlanetButton Planet
    {
        get
        {
            return planet;
        }

        set
        {
            planet = value;
        }
    }

    public int GetKilledEnemies ()
    {
        return KilledEnemies;
    }
   



    public void Start()
    {
        planetBase = GameObject.FindGameObjectWithTag("Base");
        score = 0;
        activeWinConditions = new List<WinCondition>();
        possibleWinConditions = new List<WinCondition>() {
            new Exterminate(25, this, 0), new TimeTrial(45, this, 0)
        };
        KilledEnemies = 0;
        galaxyMap.SetActive(true);
        if(galaxyMap.activeSelf)
        {
            player.GetComponent<Weapon>().enabled = false;
            Time.timeScale = 0;
            canPause = false;
        }
        StartCoroutine(ScanNavGrid());
    }

    public IEnumerator NewLevel(PlanetButton planet= null, Sprite planetSprite = null)
    {
        foreach (GameObject objectiveIndicator in objectiveIndicators)
        {
            objectiveIndicator.SetActive(false);
        }
        activeWinConditions.Clear();
        possibleWinConditions = new List<WinCondition>() {
            new Exterminate(60, this, 0), new TimeTrial(90, this, 0)
        };
        int objectiveCount = UnityEngine.Random.Range(1, objectiveIndicators.Length+1);
        for (int i = 0; i < objectiveCount; i++)  
        {
            int indexNr = UnityEngine.Random.Range(0, possibleWinConditions.Count);
            activeWinConditions.Add(possibleWinConditions[indexNr]);
            activeWinConditions[i].AssignIndicator(objectiveIndicators[i]);
            activeWinConditions[i].MultiplyGoalBy(1/(objectiveCount*0.75f));
            possibleWinConditions.RemoveAt(indexNr);
        }
        
        if(planet != null)
        {
            this.planet = planet;
        }
        if (planetSprite != null)
        {
            planetBase.GetComponent<Base>().SetSprite(planetSprite);
        }
        Restart();
        if (galaxyMap.activeSelf)
        {
            Time.timeScale = 1;
            yield return new WaitForSeconds(1);
            galaxyMap.SetActive(false);
            canPause = true;
        }
        AstarPath.active.Scan();
        GetComponent<Music>().PlayTrack();
    }

    IEnumerator ScanNavGrid ()
    {
        yield return new WaitForSeconds(0.2f);
        AstarPath.active.Scan();
        StartCoroutine(ScanNavGrid());
    }

    public void Options ()
    {
        if(!OptionsMenu.activeSelf)
        {
            OptionsMenu.SetActive(true);
        }
        else
        {
            OptionsMenu.SetActive(false);
        }
    }

    public void Update()
    {
        foreach(WinCondition winCondition in activeWinConditions)
        {
            if(winCondition != null)
                winCondition.UpdateCondition();
        }  
        if (Input.GetButtonDown("Cancel") && !PauseMenu.activeSelf )
        {
            Pause();
        }
        else if (Input.GetButtonDown("Cancel") && PauseMenu.activeSelf && !hasWon && !hasLost)
        {
            Resume();
        }
    }

    public void GameOverMenu ()
    {
        Debug.Log("GameOverMenu");
        gameOverMenu.SetActive(true);
        Time.timeScale = 0;
        player.GetComponent<Weapon>().enabled = false;
    }

    public void GameOver(string reason = "")
    {
        if(!hasLost)
        {
            hasLost = true;
            GameOverMenu();
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
        bool shouldWin = true;
        foreach (WinCondition winCondition in activeWinConditions)
        {
            if(!winCondition.achieved)
            {
                shouldWin = false;
            }
        }
        if (!hasWon && shouldWin)
        {
            hasWon = true;
            planet.CompleteMission();
            player.GetComponent<PlayerStats>().TriggerAnim("Win");
        }
    }

    public void WinMenu()
    {
        winMenu.SetActive(true);
        Time.timeScale = 0;
        foreach (Button elem in winMenu.GetComponentsInChildren<Button>())
        {
            if (elem.transform.parent.gameObject.name == "RestartButton")
            {
                if(planet.completedMissions >= 3)
                {
                    elem.interactable = false;
                    elem.GetComponentInChildren<TextMeshProUGUI>().text = "All missions completed";
                }
                else
                {
                    elem.interactable = true;
                    elem.GetComponentInChildren<TextMeshProUGUI>().text = "Next Level";
                }
            }
        }
        foreach (TextMeshProUGUI elem in winMenu.GetComponentsInChildren<TextMeshProUGUI>())
        {
            if (elem.transform.parent.gameObject.name == "Header")
            {
                elem.text = "Victory\nMissions Completed " + planet.completedMissions + "/" + planet.missionsGoal;
            }
        }
        player.GetComponent<Weapon>().enabled = false;
    }

    public void startNewLevelCoroutine()
    {
        StartCoroutine(NewLevel());
    }

    public void TriggerGalaxyMap ()
    {
        gameOverMenu.SetActive(false);
        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        winMenu.SetActive(false);
        if(galaxyMap.activeSelf)
        {
            Time.timeScale = 1;
            galaxyMap.SetActive(false);
            canPause = true;
        }
        else
        {
            Time.timeScale = 0;
            galaxyMap.SetActive(true);
            canPause = false;
        }
        GetComponent<Music>().PlayTrack();
    }

    public void ResetLevel()
    {
        foreach (GameObject obstacle in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            Destroy(obstacle);
        }
        foreach (Transform spawnPoint in planet.obstacleSpawnPoints.GetComponentInChildren<Transform>())
        {
            if (spawnPoint.name.Contains("Point"))
            {
                int prefabIndex = UnityEngine.Random.Range(0, planet.obstaclePrefab.Length);
                Instantiate(planet.obstaclePrefab[prefabIndex], (Vector2)spawnPoint.position, spawnPoint.rotation);
            }
        }
        foreach (GameObject bullet in GameObject.FindGameObjectsWithTag("Bullet"))
        {
            Destroy(bullet);
        }
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.GetComponent<Enemy>().Die(false);
        }
        player.GetComponent<PlayerStats>().Reset();
        planetBase.GetComponent<Base>().Reset();
        KilledEnemies = 0;
        foreach (WinCondition winCondition in activeWinConditions)
        {
            if(winCondition != null)
                winCondition.ResetCondition();
        }
        hasLost = false;
        hasWon = false;

    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Resume()
    {
        winMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        PauseMenu.SetActive(false);
        Time.timeScale = 1;
        player.GetComponent<Weapon>().enabled = true;
    }

    public void Pause(string message = "Paused")
    {
        if (OptionsMenu.activeSelf)
        {
            OptionsMenu.SetActive(false);
        }
        if (canPause)
        {
            PauseMenu.SetActive(true);
            Time.timeScale = 0;
            player.GetComponent<Weapon>().enabled = false;
        }
    }

    public void PauseWinCondition()
    {
        foreach (WinCondition winCondition in activeWinConditions)
        {
            winCondition.PauseCondition();
        }
        
    }
    public void ResumeWinCondition()
    {
        foreach (WinCondition winCondition in activeWinConditions)
        {
            winCondition.ResumeCondition();
        }
    }
}