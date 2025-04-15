using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour, IAudioObserver
{
    [SerializeField] public float moveSpeed = 14f;
    [SerializeField] float enemyMoveX = 100.0f;
    [SerializeField] float enemyMoveY = 0f;
    private float fireRate = 0.5f;
    private float specialShieldRate = 3f;
    private float lastFireTime = 0f;
    private float lastSpecialShieldTime = 0f;

    public float jumpAmount = 7f;

    private Animator animator;
    PlayerStats playerstats;
    public Rigidbody2D rb;
    bool isGrounded;
    bool right;
    public float groundCheckDistance = 1.5f;
    public LayerMask groundLayer;

    [SerializeField] private AudioClip jumpSFX;
    [SerializeField] private AudioClip fireSFX;
    [SerializeField] private AudioClip shieldSFX;

    private IShootStrategy normalShootStrategy;
    private IShootStrategy specialShieldStrategy;

    public GameObject shootingController;
    public GameObject shieldController;

    //Interactables
    private IInteractable currentInteractable;

    //Touch input
    private Vector2 touchStartPos;
    private bool isTouching;
    private float tapStartTime;

    //Movimiento tactil
    private float moveInputTouch = 0f;

    void OnDestroy()
    {
        SFX_Driver.Instance.RemoveObserver(this);
    }

    public void OnSoundPlayed(AudioClip audioClip)
    {
        SFX_Driver.Instance.PlaySound(audioClip);
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        playerstats = GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody2D>();

        SFX_Driver.Instance.RegisterObserver(this);

        normalShootStrategy = shootingController.GetComponent<Fire>();
        specialShieldStrategy = shieldController.GetComponent<FireShield>();
    }

    void Update()
    {
        if (playerstats.blockkeys == false)
        {
            HandleMovement();
            HandleTouchInput();
            HandleInteraction();
        }
    }

    void HandleMovement()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);

        float moveDirection = moveInputTouch;

        //float moveDirection = 0f;

        ////En movil usar moveInputTouch
        //if (Application.isMobilePlatform)
        //{
        //    moveDirection = moveInputTouch;
        //}
        //else
        //{
        //    //En editor o standalone usar teclas
        //    moveDirection = Input.GetAxisRaw("Horizontal");
        //}

        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);

        if (moveDirection > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            animator.SetTrigger("startMove");
            right = true;
        }
        else if (moveDirection < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            animator.SetTrigger("startMove");
            right = false;
        }
        else
        {
            animator.SetTrigger("notMove");
        }
    }


    void HandleTouchInput()
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

        //Touch Input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = touch.position;

            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touchPos;
                tapStartTime = Time.time;
                isTouching = true;
            }

            if (touch.phase == TouchPhase.Moved && isTouching)
            {
                HandleSwipe(touchPos - touchStartPos, touchStartPos.x > screenCenter.x);
            }

            if (touch.phase == TouchPhase.Ended && isTouching)
            {
                HandleTap(touchStartPos.x > screenCenter.x, Time.time - tapStartTime);
                isTouching = false;
            }
        }

        //Mouse Input (simulacion)
        else if (Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            if (Input.GetMouseButtonDown(0))
            {
                touchStartPos = Input.mousePosition;
                tapStartTime = Time.time;
                isTouching = true;
            }

            if (Input.GetMouseButton(0) && isTouching)
            {
                Vector2 swipeDelta = (Vector2)Input.mousePosition - touchStartPos;
                HandleSwipe(swipeDelta, touchStartPos.x > screenCenter.x);

            }

            if (Input.GetMouseButtonUp(0) && isTouching)
            {
                float tapDuration = Time.time - tapStartTime;
                HandleTap(touchStartPos.x > screenCenter.x, tapDuration);
                isTouching = false;
            }
        }
    }

    void HandleSwipe(Vector2 swipeDelta, bool isRightSide)
    {
        if (!isRightSide) return;

       
        float sensitivityFactor = 2f;
        swipeDelta *= sensitivityFactor;

        if (swipeDelta.magnitude > 50f)
        {
            if (swipeDelta.y > 0 && isGrounded)
            {
                //Salta vertical nomas
                rb.AddForce(new Vector2(0f, jumpAmount), ForceMode2D.Impulse);
                OnSoundPlayed(jumpSFX);


            }
            else if (swipeDelta.y < 0 && currentInteractable != null)
            {
                currentInteractable.Interact();
            }

            isTouching = false;
        }
    }

    void HandleTap(bool isRightSide, float tapDuration)
    {
        if (!isRightSide) return;

        if (tapDuration < 0.3f)
        {
            if (Time.time - lastFireTime >= fireRate)
            {
                normalShootStrategy.Shoot(50, transform, right);
                OnSoundPlayed(fireSFX);
                lastFireTime = Time.time;
            }
        }
        else
        {
            if (Time.time - lastSpecialShieldTime >= specialShieldRate)
            {
                specialShieldStrategy.Shoot(10, transform, right);
                OnSoundPlayed(shieldSFX);
                lastSpecialShieldTime = Time.time;
            }
        }
    }


    void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    public void MoveLeftTouch(bool isPressed)
    {
        moveInputTouch = isPressed ? -1f : 0f;
    }

    public void MoveRightTouch(bool isPressed)
    {
        moveInputTouch = isPressed ? 1f : 0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            animator.SetTrigger("Damage");

            if (right)
                transform.Translate(Time.deltaTime * -enemyMoveX, enemyMoveY, 0);
            else
                transform.Translate(Time.deltaTime * enemyMoveX, enemyMoveY, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var interactable = collision.GetComponent<IInteractable>();
        if (interactable != null)
        {
            currentInteractable = interactable;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var interactable = collision.GetComponent<IInteractable>();
        if (interactable != null && interactable == currentInteractable)
        {
            currentInteractable = null;
        }
    }
}
