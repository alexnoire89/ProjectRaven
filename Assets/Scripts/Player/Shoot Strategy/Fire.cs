using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour,IShootStrategy
{

    [SerializeField] float powerFire = 20f;
    [SerializeField] float fireTime = 2f;

   

  
    float currentTime;

    public GameObject firePrefab;

  

    public int damageFire;

    public Fire(GameObject prefab)
    {
        firePrefab = prefab;
    }


    public void Shoot(int damage, Transform transform, bool right)
    {

        GameObject fireInstance = Instantiate(firePrefab, transform.position, transform.rotation);
        Rigidbody2D fireRigidbody = fireInstance.GetComponent<Rigidbody2D>();

        if (fireRigidbody != null)
        {

            Fireball(right, fireRigidbody);
        }

        this.damageFire = damage;
    }



    void Update()
    {

        //Sumador de tiempo
        currentTime += Time.deltaTime;

        //tiempo de vida del disparo
        if (currentTime > fireTime)
        {
            Destroy(gameObject);
        }

    }



    //funcion para disparar
    public void Fireball(bool right, Rigidbody2D fireRigidbody)
    {
        if (right){

            fireRigidbody.AddForce(Vector2.right * powerFire, ForceMode2D.Impulse);
            
              
        }
        else
        {
            fireRigidbody.AddForce(Vector2.left * powerFire, ForceMode2D.Impulse);
         
        }
       
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {

        //Si las balas tocan el layer del piso se destruyen
        if (collision.gameObject.layer == 6)
        {
            Destroy(gameObject);
            
        }


        //if (collision.gameObject.GetComponent<Enemy>() != null)

            if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemyScript = collision.gameObject.GetComponent<Enemy>();

            if (enemyScript != null)
            {
                enemyScript.TakeDamage(damageFire);

                Destroy(gameObject);
            }
        }


        if (collision.gameObject.CompareTag("FragileDoor"))
        {
            FragileDoor fragileDoorScript = collision.gameObject.GetComponent<FragileDoor>();

            if (fragileDoorScript != null)
            {
              
                fragileDoorScript.TakeDamage(damageFire);
                Destroy(gameObject);
            }
        }


        if (collision.gameObject.CompareTag("Boss"))
        {
            Boss_Skelleton bossScript = collision.gameObject.GetComponent<Boss_Skelleton>();

            if (bossScript != null)
            {
                bossScript.GetDamaged(damageFire);

                Destroy(gameObject);
            }
        }


    }

}
