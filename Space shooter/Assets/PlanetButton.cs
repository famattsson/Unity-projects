using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlanetButton : MonoBehaviour {

    public GameObject InfoBox;
    public Animator animator;
    public Sprite planetSprite;
    GameManager gameManager;
    public int completedMissions;
    public int missionsGoal;
    public TextMeshProUGUI completedMissionsText;
    public AudioClip[] audioClips;
    public GameObject obstacleSpawnPoints;
    public GameObject[] obstaclePrefab;
    public List<Sprite> stateSprites;

    void Start ()
    {
        completedMissions = 0;
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        completedMissionsText.text = "COMPLETED MISSIONS " + completedMissions + "/" + missionsGoal;
        UpdateStateSprite();
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel") && InfoBox.activeSelf)
        {
            animator.ResetTrigger("Highlighted");
            animator.ResetTrigger("Disabled");
            animator.ResetTrigger("Pressed");
            animator.ResetTrigger("Normal");
            animator.SetTrigger("Back");
            
        } 
    }

    void UpdateStateSprite ()
    {
        GetComponent<Image>().sprite = stateSprites[completedMissions];
    }

    public void CompleteMission ()
    {
        completedMissions = Mathf.Clamp(completedMissions + 1, 0 , missionsGoal);
        completedMissionsText.text = "COMPLETED MISSIONS " + completedMissions + "/" + missionsGoal;
        UpdateStateSprite();
    }

    public void CoverOther ()
    {
        
        gameObject.transform.SetAsLastSibling();
    }

    public void LaunchMission ()
    {
        animator.ResetTrigger("Highlighted");
        animator.ResetTrigger("Disabled");
        animator.ResetTrigger("Pressed");
        animator.ResetTrigger("Normal");
        animator.SetTrigger("Back");
        StartCoroutine(gameManager.NewLevel(this, planetSprite));
    }
}