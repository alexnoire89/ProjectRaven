using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBall : MonoBehaviour
{

    public GameObject baseball;


    private void OnTriggerEnter2D(Collider2D collision)
    {

        //Si el PJ se para en el mosaico destruye la base que mantiene la bola
        if (collision.gameObject.tag == "Player")
        {
            Destroy(GameObject.FindGameObjectWithTag("baseball"));
        }



    }

}
