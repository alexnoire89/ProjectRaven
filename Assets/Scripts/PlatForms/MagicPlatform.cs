using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPlatform : MonoBehaviour
{

    [SerializeField] public float startTime = 0;
    [SerializeField] public float showTime = 2;

    float currentTime;
    public GameObject platform;

    void Start()
    {
        currentTime += startTime;
      
    }

    void Update()
    {
       
        PlatformCycle();


        
    }

    void PlatformCycle()
    {
        currentTime += Time.deltaTime;


        if (platform.activeInHierarchy)
        {
            if (currentTime > showTime)
            {
                platform.SetActive(false);
                currentTime = 0;
            }
        }
        else
        {
            if (currentTime > showTime)
            {
                platform.SetActive(true);
                currentTime = 0;
            }
        }

  
    }

}
