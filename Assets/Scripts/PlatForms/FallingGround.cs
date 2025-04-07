using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingGround : MonoBehaviour
{
    [SerializeField] float limitTime = 1f;
    [SerializeField] float speedFall = 1f;

    bool onGround = false;
    float onTime;


    void Update()
    {
        //tiempo para la rotacion del sprite



        if (onGround)
        {
            //Sumador de tiempo
            onTime += Time.deltaTime;

       
            transform.Translate(0, Time.deltaTime * -speedFall, 0);




        }

        //si se vence el tiempo de estar sobre el piso se destruye
        if (onTime > limitTime)
        {
            Destroy(gameObject);
        }
    }


    private void OnCollisionStay2D(Collision2D collision)
    {

        //Si el PJ se para en el mosaico se va a caer
        if (collision.gameObject.layer == 7)
        {
            onGround = true;
        }



    }

}
