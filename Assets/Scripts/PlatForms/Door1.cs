using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Door1 : MonoBehaviour
{
    [SerializeField] private float moveDistance = 8f;
    [SerializeField] private float speedDoor = 2f;
    [SerializeField] private bool moveInX = false; // Por defecto se mueve en Y

    private bool isOpening = false;
    private Vector3 targetPosition;

    void Start()
    {
        if (moveInX)
        {
            targetPosition = transform.position - new Vector3(moveDistance, 0, 0);
        }
        else
        {
            targetPosition = transform.position - new Vector3(0, moveDistance, 0);
        }
    }

    void Update()
    {
        if (isOpening)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speedDoor * Time.deltaTime);
        }
    }

    public void OpenDoor()
    {
        isOpening = true;
    }
}


