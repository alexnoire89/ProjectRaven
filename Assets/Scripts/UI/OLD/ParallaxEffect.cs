using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private Vector2  parallaxEffectMultiplier;

    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    private float width;

   // public Camera camera; // you will need to set this in the inspector


    void Start()
    {
        cameraTransform = Camera.main.transform; 
        lastCameraPosition = cameraTransform.position;
        width = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    private void FixedUpdate()

    {

        if (GetComponent<Camera>() != null)
        {
            transform.LookAt(GetComponent<Camera>().transform);
        }


        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, 0, 0);
        lastCameraPosition = cameraTransform.position;

        float distanceWithTheCamera = cameraTransform.position.x - transform.position.x;

        if(Mathf.Abs(distanceWithTheCamera) >= width)
        {

            var movement = distanceWithTheCamera > 0 ? width * 2f : width * -2f;
            transform.position = new Vector3(transform.position.x + movement, transform.position.y, 0);



        }

    }
}
