using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireShield : MonoBehaviour, IShootStrategy
{

    [SerializeField] float shieldTime = 4f;
    float currentTime;

    public GameObject fireShieldPrefab;

    public int damageFire;


    public FireShield(GameObject prefab)
    {
        fireShieldPrefab = prefab;
    }


    public void Shoot(int damage, Transform transform, bool right)
    {

        GameObject fireInstance = Instantiate(fireShieldPrefab, transform.position, transform.rotation);

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

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemyScript = other.GetComponent<Enemy>();

            if (enemyScript != null)
            {
                enemyScript.TakeDamage(damageFire);

              
            }
        }


        if (other.CompareTag("Boss"))
        {
            Boss_Skelleton bossScript = other.GetComponent<Boss_Skelleton>();

            if (bossScript != null)
            {
                bossScript.GetDamaged(damageFire);

            }
        }

    }





}
