using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FloatingSprite : MonoBehaviour
{
    [Header("Movimiento vertical (flotación)")]
    public float floatAmplitude = 10f;
    public float floatFrequency = 1f;

    [Header("Movimiento horizontal aleatorio")]
    public float maxHorizontalOffset = 30f;       // Distancia máxima desde el centro
    public float horizontalSpeed = 1f;            // Velocidad del movimiento lateral

    private Vector3 startPosition;
    private float currentTargetOffsetX;
    private float currentOffsetX;

    void Start()
    {
        startPosition = transform.localPosition;
        SetNewTargetOffset();
    }

    void Update()
    {
        // Movimiento vertical (flotación senoidal)
        float yOffset = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;

        // Movimiento horizontal (lerp suave hacia un nuevo punto aleatorio)
        currentOffsetX = Mathf.MoveTowards(currentOffsetX, currentTargetOffsetX, horizontalSpeed * Time.deltaTime);

        // Si llegamos al objetivo, elegimos uno nuevo aleatorio
        if (Mathf.Approximately(currentOffsetX, currentTargetOffsetX))
        {
            SetNewTargetOffset();
        }

        // Posición final combinada
        transform.localPosition = startPosition + new Vector3(currentOffsetX, yOffset, 0f);
    }

    void SetNewTargetOffset()
    {
        currentTargetOffsetX = Random.Range(-maxHorizontalOffset, maxHorizontalOffset);
    }
}

