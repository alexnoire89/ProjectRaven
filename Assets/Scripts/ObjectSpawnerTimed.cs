using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ObjectSpawnerTimed : MonoBehaviour
{
    [Header("Configuracion")]
    public GameObject prefabToSpawn;
    public Transform spawnPoint;
    public float spawnDelay = 0f;       
    public float lifetime = 5f;
    float timer = 0f;
    private bool doOnce =false;


    private IEnumerator SpawnAndDestroy()
    {
        if (spawnDelay > 0f)
            yield return new WaitForSeconds(spawnDelay);

        GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);

        yield return new WaitForSeconds(lifetime);

        if (spawnedObject != null)
            Destroy(spawnedObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
            if (collision.CompareTag("Player"))
            {
           
                timer += Time.deltaTime;

            if (!doOnce)
            {
                GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
                doOnce = true;  
            }
                
            if(timer > spawnDelay)
            {
                StartCoroutine(SpawnAndDestroy());
                timer = 0f;
            }
               
            }

        
    }
}

