using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleElement : MonoBehaviour
{

    public void TakeDamage()
    {
        Destroy(gameObject);
    }
}
