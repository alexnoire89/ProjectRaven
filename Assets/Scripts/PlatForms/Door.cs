using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    [SerializeField] public float downLimitDoor = 8;
    [SerializeField] public float speedDoor= 2;
    private float positionLimitY;

    public void Start()
    {
        positionLimitY = transform.position.y - downLimitDoor;
    }

    public void OpenDoor()
    {
        //Baja la puerta hasta la posicion del piso
        if(transform.position.y > positionLimitY) {

            transform.Translate(0, Time.deltaTime * -speedDoor, 0);
        
        }
        
    }
}
