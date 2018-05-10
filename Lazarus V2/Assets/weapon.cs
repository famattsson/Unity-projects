using UnityEngine;
using System.Collections;

public class weapon : MonoBehaviour {

    public float firerate = 0;
    public float damage = 10;
    public LayerMask whatToHit;
    public float Firerange=100;
    float TimeToSpawnEffect = 0;
    float EffectSpawnRate = 10;

    public Transform Bullettrailprefab;
    public Transform Muzzleflashprefab;

    float timeToFire = 0;
    Transform Firepoint;

	// Use this for initialization
	void Awake ()
    {
        Firepoint = transform.Find("Firepoint");
        if (Firepoint == null)
        {
            Debug.LogError("Aint no fucking firepoint!");
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
       
        if (firerate == 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot ();
            }
        }
        else 
        {
            if(Input.GetButton("Fire1") && Time.time > timeToFire)
            {
                timeToFire = Time.time + 1/firerate;
                Shoot (); 
            }
        }    
	}

    void Shoot ()
    {
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 FirepointPosition = new Vector2(Firepoint.position.x, Firepoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast(FirepointPosition, mousePosition - FirepointPosition, 100, whatToHit);
        if (Time.time >= TimeToSpawnEffect)
        {
            Effect();
            TimeToSpawnEffect = Time.time + 1 / EffectSpawnRate;
        }
      //  Debug.DrawLine(FirepointPosition, (mousePosition-FirepointPosition)*100, Color.cyan);
      //  if (hit.collider != null)
      //{
      //      Debug.Log("hit " + hit.collider.name+ " and did "+ damage + " damage");
      //      Debug.DrawLine(FirepointPosition, hit.point, Color.red);
      //  }
    }
    void Effect ()
    {
        Instantiate(Bullettrailprefab, Firepoint.position, Firepoint.rotation);
       Transform clone = (Transform) Instantiate (Muzzleflashprefab, Firepoint.position, Firepoint.rotation);
        clone.parent = Firepoint;
        float size = Random.Range(0.6f, 0.9f);
        clone.localScale = new Vector3(size, size, size);
        Destroy (clone.gameObject, 0.02f);
    }
}
