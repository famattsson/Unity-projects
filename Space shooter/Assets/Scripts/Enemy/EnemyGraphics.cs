using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGraphics : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TriggerDeath (int isOutOfBounds = 1)
    {
        if(isOutOfBounds == 0)
        {
            GetComponentInParent<Enemy>().Die(true,false);
        }
        else if (isOutOfBounds == 1)
        {
            Debug.Log(isOutOfBounds);
            GetComponentInParent<Enemy>().Die(false,true);
        }
    }
}
