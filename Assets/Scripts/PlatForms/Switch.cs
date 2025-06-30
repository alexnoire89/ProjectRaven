using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour, IInteractable, IAudioObserver
{
    bool isClosed = false;
    bool isOpen = false;
    bool playOnce = true;
    [SerializeField] private GameObject hitParticles;

    public Door door;
    [SerializeField] private AudioClip doorSFX;
    private Animator animator;

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
        door = GameObject.FindGameObjectWithTag("Door").GetComponent<Door>();
        SFX_Driver.Instance.RegisterObserver(this);
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isOpen && playOnce)
        {
            OnSoundPlayed(doorSFX);
            playOnce = false;

            if (hitParticles != null)
            {
                GameObject particles = Instantiate(hitParticles, transform.position, Quaternion.identity);
                Destroy(particles, 1.5f); // evita dejar basura en el editor
            }

        }
    }

    //metodo para la interaccion
    public void Interact()
    {
        if (isClosed)
        {
            animator.SetTrigger("isActivated");
            isOpen = true;
            door.OpenDoor();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isClosed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isClosed = false;
        }
    }
}
