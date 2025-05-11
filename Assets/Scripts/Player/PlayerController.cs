using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour, IAudioObserver
{
    #region General Stats
    [Header("Stats")]
    [SerializeField] private float moveSpeed = 14f;
    private float enemyMoveX = 40.0f;
    private float enemyMoveY = 0f;
    [SerializeField] private float fireRate = 0.5f;
    private float specialShieldRate = 3f;
    private float lastFireTime = 0f;
    private float lastSpecialShieldTime = 0f;
    [SerializeField] private float jumpAmount = 7f;
    private Animator animator;
    PlayerStats playerstats;
    public Rigidbody2D rb;
    bool isGrounded;
    bool right;
    [SerializeField] private float groundCheckDistance = 1.5f;
    [SerializeField] private LayerMask groundLayer;

    [Header("SFX")]
    [SerializeField] private AudioClip jumpSFX;
    [SerializeField] private AudioClip fireSFX;
    [SerializeField] private AudioClip shieldSFX;

    private IShootStrategy normalShootStrategy;
    private IShootStrategy specialShieldStrategy;

    [Header("Scripts")]
    public GameObject shootingController;
    public GameObject shieldController;

    //Interactables
    private IInteractable currentInteractable;
    #endregion

    #region Mobile Movement

    [Header("TouchMovement")]
    [SerializeField] private float swipeSensitivity = 2f;

    //Touch input
    private Vector2 touchStartPos;
    private bool isTouching;
    private float tapStartTime;

    //Touch Taps
    [SerializeField] RectTransform fireZone;

    //Joystick Touch
    [SerializeField] private FloatingJoystick joystick;

    //Movimiento tactil
    private float moveInputTouch = 0f;

    #endregion


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
        }
    }

    void HandleMovement()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);

        // --- Teclado solo en editor o PC ---
        if (Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            if (Input.GetKey(KeyCode.A))
                moveInputTouch = -1f;
            else if (Input.GetKey(KeyCode.D))
                moveInputTouch = 1f;
            else
                moveInputTouch = 0f; // Importante: reset si no se toca nada
        }
        else
        {
            // --- Solo si estamos en móvil se toma el input del joystick
            Vector2 joystickInput = joystick.InputDirection;
            moveInputTouch = joystickInput.x;
        }

        ////Joystick
        //Vector2 joystickInput = joystick.InputDirection;
        //moveInputTouch = joystickInput.x;




        rb.velocity = new Vector2(moveInputTouch * moveSpeed, rb.velocity.y);

        //Animaciones y Sprite
        if (moveInputTouch > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            animator.SetTrigger("startMove");
            right = true;
        }
        else if (moveInputTouch < 0)
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

        //Manejo de dedos en pantalla
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            Vector2 touchPos = touch.position;

            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touchPos;
                tapStartTime = Time.time;
                isTouching = true;
            }

            //Swipes del lado derecho de la pantalla
            if (touch.phase == TouchPhase.Moved && isTouching)
            {
                HandleSwipe(touchPos - touchStartPos, touchStartPos.x > Screen.width / 2);
            }

            //Taps
            if (touch.phase == TouchPhase.Ended && isTouching)
            {
                float tapDuration = Time.time - tapStartTime;

                HandleTap(touchStartPos, tapDuration);
                isTouching = false;
            }
        }

        //PLACEHOLDER SOLO PARA SIMULACION DEL EDITOR DE UNITY
        if ((Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer) )
        {
            Vector2 mousePos = Input.mousePosition;

    

            if (Input.GetMouseButtonDown(0))
            {
                touchStartPos = mousePos;
                tapStartTime = Time.time;
                isTouching = true;
            }

            if (Input.GetMouseButton(0) && isTouching)
            {
                Vector2 swipeDelta = (Vector2)Input.mousePosition - touchStartPos;
                HandleSwipe(swipeDelta, touchStartPos.x > Screen.width / 2);
            }

            if (Input.GetMouseButtonUp(0) && isTouching)
            {
                float tapDuration = Time.time - tapStartTime;

           

                HandleTap(touchStartPos, tapDuration);

                isTouching = false;
            }

        
        }
    }

    void HandleSwipe(Vector2 swipeDelta, bool isRightSide)
    {
        if (!isRightSide) return;

        swipeDelta *= swipeSensitivity;

        if (swipeDelta.magnitude > 50f)
        {
            //Swipe Salto
            if (swipeDelta.y > 0 && isGrounded)
            {
                rb.AddForce(new Vector2(0f, jumpAmount), ForceMode2D.Impulse);
                OnSoundPlayed(jumpSFX);
            }

            //Swipe Interacturar
            else if (swipeDelta.y < 0 && currentInteractable != null)
            {
                currentInteractable.Interact();
            }

            isTouching = false;
        }
    }

    void HandleTap(Vector2 tapPosition, float tapDuration)
    {
        if (!RectTransformUtility.RectangleContainsScreenPoint(fireZone, tapPosition)) return;

        //Tap Disparo
        if (tapDuration < 0.3f)
        {
            if (Time.time - lastFireTime >= fireRate)
            {
                normalShootStrategy.Shoot(50, transform, right);
                OnSoundPlayed(fireSFX);
                lastFireTime = Time.time;
            }
        }

        //Tap Escudo
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
