using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGate : MonoBehaviour
{
    public FinalGate finalgate;
    

    private void Start()
    {
        finalgate = GameObject.FindGameObjectWithTag("FinalGate").GetComponent<FinalGate>();
    }

   

    private void OnTriggerEnter2D(Collider2D collision)
    {

        //Si el PJ se para en el mosaico destruye la base que mantiene la bola
        if (collision.gameObject.tag == "Player")
        {
           
            finalgate.CloseDoor();
        }



    }
}
