using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireLightFlicker : MonoBehaviour
{
    [Header("Referencia")]
    public Image imageUI;                  // Para UI
    public SpriteRenderer spriteRenderer; // Para SpriteRenderer

    [Header("Configuración")]
    public float minAlpha = 0.4f;
    public float maxAlpha = 1.0f;
    public float flickerSpeed = 1.5f;  // Qué tan rápido cambia el brillo
    public float flickerVariation = 0.3f; // Cuánta variación hay (intensidad)

    private float randomOffset;

    void Start()
    {
        randomOffset = Random.Range(0f, 100f); // Para que varias llamas no sean iguales
    }

    void Update()
    {
        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, randomOffset);
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, noise * (1f - flickerVariation) + Random.Range(0f, flickerVariation));

        if (imageUI != null)
        {
            Color c = imageUI.color;
            c.a = alpha;
            imageUI.color = c;
        }

        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = alpha;
            spriteRenderer.color = c;
        }
    }
}

