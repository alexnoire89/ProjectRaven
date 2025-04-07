using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapColumn : MonoBehaviour
{

    bool onTrap = false;

    Rigidbody2D rb;


    void Update()
    {
        if (onTrap)
        {
            rb.AddForce(Vector2.left * 10, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        //Si el PJ se para en el mosaico se va a caer
        if (collision.gameObject.tag == "Player")
        {
            onTrap = true;
        }



    }
}
