using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Door : MonoBehaviour
{
    [SerializeField] public float downLimitDoor = 8;
    [SerializeField] public float speedDoor = 2;
    private float positionLimitY;
    private bool isOpening = false;

    void Start()
    {
        positionLimitY = transform.position.y - downLimitDoor;
    }

    void Update()
    {
        if (isOpening && transform.position.y > positionLimitY)
        {
            transform.Translate(0, Time.deltaTime * -speedDoor, 0);
        }
    }

    public void OpenDoor()
    {
        isOpening = true;
    }
}


