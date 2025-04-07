using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Chest : MonoBehaviour
{

    [SerializeField] public PlayerStats player;

    private Animator animator;
    public GameObject scoreCoin;

    private bool isClosed = false;
    private bool NotOpenedOnce = true;
    private bool respawn = false;

    private float currentTime;

    public Vector3 minPosition;
    public Vector3 maxPosition;
    public Vector3 coinPosition;

    [SerializeField] public int minXPositionSpawn = -2;
    [SerializeField] public int maxXPositionSpawn = 2;
    [SerializeField] public int minYPositionSpawn = -1;
    [SerializeField] public int maxYPositionSpawn = 1;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        minPosition = new Vector3(transform.position.x + minXPositionSpawn, transform.position.y + minYPositionSpawn, transform.position.z);
        maxPosition = new Vector3(transform.position.x + maxXPositionSpawn, transform.position.y + maxYPositionSpawn, transform.position.z);

      


    }

    // Update is called once per frame
    void Update()
    {
        //Randomeo el transform de las monedas para que spawneen en posiciones distintas
        Vector3 randomPosition = new Vector3(Random.Range(minPosition.x, maxPosition.x), transform.position.y, transform.position.z);
        coinPosition = randomPosition;

        //Si el player esta en el cofre y ademas tiene la llave, que se abra apretando E

        if (isClosed && player.KeyA && NotOpenedOnce) {

            // Si el PJ Esta cerca del switch y presiona E se activa.
            if (Input.GetKey(KeyCode.E) && isClosed)
            {
                animator.SetTrigger("OpenChest");


                respawn = true;

                NotOpenedOnce = false;


            }

        }

        if (respawn)
        {
            currentTime += Time.deltaTime;

            if (currentTime < 1)
            {
                RespawnCoins();
            }

                
        }


    }

    private void RespawnCoins()
    {
        GameObject lc = Instantiate(scoreCoin, coinPosition, transform.rotation);
        lc.GetComponent<ScoreCoin>();
    }
    

    private void OnTriggerStay2D(Collider2D collision)
    {

        //Si el cofre toca al PJ Se activa el bool
        if (collision.gameObject.tag == "Player")
        {
            isClosed = true;
        }



    }

  


}
