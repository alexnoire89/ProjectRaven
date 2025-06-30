using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IAudioObserver
{
    [Header("Raycast Origins")]
    [SerializeField] private Transform castPointLeft;
    [SerializeField] private Transform castPointRight;

    public EnemyFlyweightData data;
    public static event Action<int> OnScoreChanged;
    private SpriteRenderer spriteRenderer;
    private float currentTime;

    public bool isIdleOnly = false;

    protected Rigidbody2D rb;
    private bool movingRight = true;
    private int enemyHP;
    private EnemyState currentState = EnemyState.Patrol;

    private Vector3 playerPosition;
    [SerializeField] private GameObject destroyParticles;


    public enum EnemyState
    {
        Patrol,
        Chase,
        Idle
    }

    protected virtual void Awake()
    {
        enemyHP = data.BaseHealth;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();


    }

    private void Start()
    {
        SFX_Driver.Instance.RegisterObserver(this);

    }

    private void Update()
    {
        if (enemyHP <= 0)
        {
            Die();
            return;
        }

        if (isIdleOnly)
        {
            currentState = EnemyState.Idle;
        }

        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Chase:
                Chase();
                break;
            case EnemyState.Idle:
                Idle();
                break;
        }

        if (!isIdleOnly)
        {
            if (DetectPlayer())
            {
                currentState = EnemyState.Chase;
            }
            else if (currentState == EnemyState.Chase)
            {
                currentState = EnemyState.Patrol;
            }
        }
    }

    private bool DetectPlayer()
    {
        RaycastHit2D hitRight = Physics2D.Raycast(castPointRight.position, Vector2.right, data.RayDistance);
        RaycastHit2D hitLeft = Physics2D.Raycast(castPointLeft.position, Vector2.left, data.RayDistance);

        Debug.DrawRay(castPointRight.position, Vector2.right * data.RayDistance, hitRight.collider != null && hitRight.collider.CompareTag("Player") ? Color.red : Color.green);
        Debug.DrawRay(castPointLeft.position, Vector2.left * data.RayDistance, hitLeft.collider != null && hitLeft.collider.CompareTag("Player") ? Color.red : Color.green);

        if (hitRight.collider != null && hitRight.collider.CompareTag("Player"))
        {
            playerPosition = hitRight.collider.transform.position;
            return true;
        }

        if (hitLeft.collider != null && hitLeft.collider.CompareTag("Player"))
        {
            playerPosition = hitLeft.collider.transform.position;
            return true;
        }

        return false;
    }


    public void Patrol()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();

        currentTime += Time.deltaTime;

        if (currentTime > data.PatrolTime)
        {
            movingRight = !movingRight;
            currentTime = 0;
        }

        float speed = movingRight ? data.PatrolSpeed : -data.PatrolSpeed;
        rb.velocity = new Vector2(speed, rb.velocity.y);

        spriteRenderer.flipX = movingRight ? data.XFlip : !data.XFlip;
    }


    private void Chase()
    {
        if (playerPosition == null) return;

        float direction = playerPosition.x - transform.position.x;



        if (direction != 0)
        {
            rb.velocity = new Vector2(Mathf.Sign(direction) * Mathf.Abs(data.MoveSpeed), rb.velocity.y);
        }

      
        spriteRenderer.flipX = direction > 0;

    }




    private void Idle()
    {
        rb.velocity = Vector2.zero;

        spriteRenderer.flipX = movingRight;
    }








    private void Die()
    {
        Instantiate(data.Lifecoin, transform.position, Quaternion.identity);
        OnScoreChanged?.Invoke(data.Enemy_Score);
        OnSoundPlayed(data.DeathSound);

        if (destroyParticles != null)
        {
            GameObject particles = Instantiate(destroyParticles, transform.position, Quaternion.identity);
            Destroy(particles, 2f); // evita dejar basura en el editor
        }


        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        enemyHP -= damage;
        currentState = EnemyState.Chase;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball") || collision.gameObject.layer == 13)
        {
            enemyHP = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("fireball"))
        {
            //enemyHP = 0;
            OnSoundPlayed(data.DeathSound);

        }
    }

    public void OnSoundPlayed(AudioClip clip)
    {
        SFX_Driver.Instance.PlaySound(clip);
    }

    private void OnDestroy()
    {
        SFX_Driver.Instance.RemoveObserver(this);
    }
}
