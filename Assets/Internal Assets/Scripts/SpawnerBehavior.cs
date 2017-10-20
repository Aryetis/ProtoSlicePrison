using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBehavior : MonoBehaviour
{
    [SerializeField] private GameObject spawnerZone;
    [SerializeField] private GameObject ennemyPrefab;
    [SerializeField] private int spawnLimit;
    [SerializeField] private GameObject ennemyFolder;

    private bool triggered = false;

	// Use this for initialization
	void Start ()
    {
		
	}
	
    void FixedUpdate()
    {
        if (triggered && spawnLimit >= 0)
        {
            Renderer spawnerZoneRenderer = spawnerZone.GetComponent<Renderer>();

            GameObject clone = Instantiate(ennemyPrefab, 
                        new Vector3(spawnerZoneRenderer.bounds.center.x+Random.Range(-spawnerZoneRenderer.bounds.size.x/2, spawnerZoneRenderer.bounds.size.x/2),
                                spawnerZoneRenderer.bounds.center.y,
                                spawnerZoneRenderer.bounds.center.z+Random.Range(-spawnerZoneRenderer.bounds.size.z/2, spawnerZoneRenderer.bounds.size.z/2)
                                ),
                        Quaternion.identity
                );

            clone.transform.parent = ennemyFolder.transform ;

            spawnLimit--;
        }
	}

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
            triggered = true;
    }
}
