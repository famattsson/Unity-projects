using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {

    public GameObject[] enemies;
    private List<SpawnObject> spawnObjects = new List<SpawnObject>();
    int count;
    private bool isPaused = false;

    class SpawnObject
    {
        public GameObject prefab;
        public float spawnTimer;
        public SpawnObject(GameObject prefab, float spawnTimer)
        {
            this.prefab = prefab;
            this.spawnTimer = spawnTimer;
        }
    }

    void Start () {
        foreach(GameObject enemy in enemies)
        {
            float timer = enemy.GetComponent<Enemy>().spawnDelay;
            SpawnObject spawnObject = new SpawnObject(enemy, timer);
            spawnObjects.Add(spawnObject);
        }
    }
	
	void FixedUpdate () {
        if(!isPaused)
        {
            for (int i = 0; i < spawnObjects.Count; i++)
            {
                SpawnObject spawnObject = spawnObjects[i];
                spawnObject.spawnTimer = Mathf.Clamp(spawnObject.spawnTimer - Time.fixedDeltaTime, 0, spawnObject.prefab.GetComponent<Enemy>().spawnDelay);

                count = (int)spawnObject.prefab.GetComponent<Enemy>().GetType().GetProperty("countGetter").GetValue(null, null);


                if (spawnObject.spawnTimer <= 0 && count < spawnObject.prefab.GetComponent<Enemy>().spawnLimit )
                {
                    Spawn(spawnObject);
                }
            } 
        }
	}

    void Spawn(SpawnObject spawnObject)
    {
        spawnObject.spawnTimer = spawnObject.prefab.GetComponent<Enemy>().spawnDelay;
        Vector2 spawnPoint = Camera.main.ViewportToWorldPoint(new Vector2(Random.Range(Camera.main.rect.xMin, Camera.main.rect.xMax), Camera.main.rect.yMax));
        Instantiate(spawnObject.prefab, spawnPoint, new Quaternion(0, 0, 180, 1));
    }

    public void PauseSpawn()
    {
        isPaused = true;
    }

    public void ResumeSpawn()
    {
        isPaused = false;
        foreach (SpawnObject elem in spawnObjects)
        {
            elem.spawnTimer = elem.prefab.GetComponent<Enemy>().spawnDelay;
        }
    }
}
