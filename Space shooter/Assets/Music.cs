using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour {

    public AudioClip[] gMAudioClips;
    public AudioSource audioSource;
    GameManager gameManager;

	void Start () {
        gameManager = GetComponent<GameManager>();
        PlayTrack();
	}
	
	void Update () {
		if(!audioSource.isPlaying)
        {
            PlayTrack();            
        }
	}

    public void PlayTrack ()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(SelectClip());
    }

    AudioClip SelectClip()
    {
        if(gameManager.galaxyMap.activeSelf)
        {
            return gMAudioClips[Random.Range(0, gMAudioClips.Length)];
        }
        else
        {
            if(gameManager.Planet != null)
            return gameManager.Planet.audioClips[Random.Range(0, gameManager.Planet.audioClips.Length)];
        }
        return null;
    }
}
