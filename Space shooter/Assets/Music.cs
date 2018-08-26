using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour {

    public AudioClip[] audioClips;
    public AudioSource audioSource;

	// Use this for initialization
	void Start () {
        audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Length - 1)]);
	}
	
	// Update is called once per frame
	void Update () {
		if(!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Length - 1)]);
        }
	}
}
