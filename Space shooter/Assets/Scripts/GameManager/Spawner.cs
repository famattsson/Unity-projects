using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject prefab;
    public float spawnDelay = 1;
    private float spawnTimer;
    public int limit = 10;
    int count;

    void Start () {
        spawnTimer = spawnDelay;
    }
	
	void FixedUpdate () {
        spawnTimer -= Time.fixedDeltaTime;
        count = (int)prefab.GetComponent<Enemy>().GetType().GetProperty("countGetter").GetValue(null, null);

        if (spawnTimer <= 0 && count < limit)
        {
            Spawn();
        }
	}

    void Spawn()
    {
        spawnTimer = spawnDelay;
        Vector2 spawnPoint = Camera.main.ViewportToWorldPoint(new Vector2(Random.Range(Camera.main.rect.xMin, Camera.main.rect.xMax), Camera.main.rect.yMax));
        Instantiate(prefab, spawnPoint, new Quaternion(0, 0, 180, 1));
    }
}
