using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextFlickerEffect : MonoBehaviour
{
    [Header("Referencia")]
    public TextMeshProUGUI tmpText; 

    [Header("Configuración")]
    public float minAlpha = 0.2f;
    public float maxAlpha = 1f;
    public float flickerSpeed = 2f;        
    public float flickerVariation = 0.3f;  

    private float randomOffset;

    void Start()
    {
        if (tmpText == null)
            tmpText = GetComponent<TextMeshProUGUI>();

        randomOffset = Random.Range(0f, 100f);
    }

    void Update()
    {
        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, randomOffset);
        float variation = Random.Range(0f, flickerVariation);
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, noise * (1f - flickerVariation) + variation);

        Color c = tmpText.color;
        c.a = alpha;
        tmpText.color = c;

        //Apagon repentino
        if (Random.value < 0.05f)
        {
            alpha = 0f; 
        }
    }
}

