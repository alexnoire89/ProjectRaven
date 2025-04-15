using System.Collections;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerStats player;
    [SerializeField] private GameObject scoreCoin;

    private Animator animator;

    private bool isClosed = false;
    private bool notOpenedOnce = true;
    private bool respawn = false;

    private float currentTime;

    private Vector3 minPosition;
    private Vector3 maxPosition;
    private Vector3 coinPosition;

    [SerializeField] private int minXPositionSpawn = -2;
    [SerializeField] private int maxXPositionSpawn = 2;
    [SerializeField] private int minYPositionSpawn = -1;
    [SerializeField] private int maxYPositionSpawn = 1;

    void Start()
    {
        animator = GetComponent<Animator>();

        minPosition = new Vector3(transform.position.x + minXPositionSpawn, transform.position.y + minYPositionSpawn, transform.position.z);
        maxPosition = new Vector3(transform.position.x + maxXPositionSpawn, transform.position.y + maxYPositionSpawn, transform.position.z);
    }

    void Update()
    {
        if (respawn)
        {
            currentTime += Time.deltaTime;

            if (currentTime < 1)
            {
                RespawnCoins();
            }
        }
    }

    public void Interact()
    {
        //Si el player tiene llave y todavia no se abrio
        if (isClosed && player.KeyA && notOpenedOnce)
        {
            animator.SetTrigger("OpenChest");
            respawn = true;
            notOpenedOnce = false;
        }
    }

    private void RespawnCoins()
    {
        Vector3 randomPosition = new Vector3(Random.Range(minPosition.x, maxPosition.x), transform.position.y, transform.position.z);
        coinPosition = randomPosition;

        GameObject lc = Instantiate(scoreCoin, coinPosition, transform.rotation);
        lc.GetComponent<ScoreCoin>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isClosed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isClosed = false;
        }
    }
}
