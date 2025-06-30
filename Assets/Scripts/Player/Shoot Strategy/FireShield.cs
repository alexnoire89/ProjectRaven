using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireShield : MonoBehaviour, IShootStrategy
{

    [SerializeField] float shieldTime = 4f;
    [SerializeField] float offsetDistance = 40f;
    float currentTime;

    public GameObject fireShieldPrefab;
    [SerializeField] private GameObject hitParticles;
    [SerializeField] private GameObject hitEnemyParticles;
    public int damageFire;


    public FireShield(GameObject prefab)
    {
        fireShieldPrefab = prefab;
    }


    public void Shoot(int damage, Transform transform, bool right)
    {
       
        //Determina la direccion en X
        Vector3 offset = new Vector3(right ? offsetDistance : -offsetDistance, 0f, 0f);

        //Calcula la nueva posicin
        Vector3 spawnPosition = transform.position + offset;
        Quaternion rotation = right ? Quaternion.identity : Quaternion.Euler(0f, 180f, 0f);

        GameObject fireInstance = Instantiate(fireShieldPrefab, spawnPosition, rotation);

        this.damageFire = damage;

    }



    void Update()
    {

        //Sumador de tiempo
        currentTime += Time.deltaTime;

        //tiempo de vida del disparo
        if (currentTime > shieldTime)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Destructible"))
        {
            DestructibleElement DEScript = other.GetComponent<DestructibleElement>();

            if (DEScript != null)
            {
                if (hitParticles != null)
                {
                    GameObject particles = Instantiate(hitParticles, transform.position, Quaternion.identity);
                    Destroy(particles, 1.5f); // evita dejar basura en el editor
                }

                DEScript.TakeDamage();


            }
        }

        if (other.gameObject.CompareTag("FragileDoor"))
        {
            FragileDoor fragileDoorScript = other.gameObject.GetComponent<FragileDoor>();

            if (fragileDoorScript != null)
            {
                if (hitParticles != null)
                {
                    GameObject particles = Instantiate(hitParticles, transform.position, Quaternion.identity);
                    Destroy(particles, 1.5f); // evita dejar basura en el editor
                }

                fragileDoorScript.TakeDamage(damageFire);
                
            }
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemyScript = other.GetComponent<Enemy>();

            if (enemyScript != null)
            {
                if (hitEnemyParticles != null)
                {
                    GameObject particles = Instantiate(hitEnemyParticles, transform.position, Quaternion.identity);
                    Destroy(particles, 1.5f); // evita dejar basura en el editor
                }

                enemyScript.TakeDamage(damageFire);

              
            }
        }


        if (other.CompareTag("Boss"))
        {
            Boss_Skelleton bossScript = other.GetComponent<Boss_Skelleton>();

            if (bossScript != null)
            {
                if (hitEnemyParticles != null)
                {
                    GameObject particles = Instantiate(hitEnemyParticles, transform.position, Quaternion.identity);
                    Destroy(particles, 1.5f); // evita dejar basura en el editor
                }

                bossScript.GetDamaged(damageFire);

            }
        }

    }





}
