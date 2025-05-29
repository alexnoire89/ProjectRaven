using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UIScrollLayer
{
    public RectTransform[] parts;       // 2 imágenes UI por capa
    public float speed = 100f;          // velocidad en píxeles/segundo
    public Vector2 direction = Vector2.left; // dirección del scroll

    [HideInInspector] public float width;
}

public class ParallaxInfiniteScroll : MonoBehaviour
{
    public List<UIScrollLayer> layers = new List<UIScrollLayer>();

    void Start()
    {
        foreach (var layer in layers)
        {
            if (layer.parts.Length >= 2)
            {
                // Asume que ambas partes tienen el mismo ancho
                layer.width = layer.parts[0].rect.width;

                // Asegura que estén colocadas una al lado de la otra
                layer.parts[0].anchoredPosition = Vector2.zero;
                layer.parts[1].anchoredPosition = new Vector2(layer.width, 0f);
            }
        }
    }

    void Update()
    {
        float deltaTime = Time.deltaTime;

        foreach (var layer in layers)
        {
            foreach (RectTransform part in layer.parts)
            {
                part.anchoredPosition += layer.direction.normalized * layer.speed * deltaTime;
            }

            RectTransform left = layer.parts[0];
            RectTransform right = layer.parts[1];

            if (layer.direction.x < 0 && left.anchoredPosition.x <= -layer.width)
            {
                left.anchoredPosition = new Vector2(right.anchoredPosition.x + layer.width, left.anchoredPosition.y);
                // Reordena
                layer.parts[0] = right;
                layer.parts[1] = left;
            }
            else if (layer.direction.x > 0 && right.anchoredPosition.x >= layer.width)
            {
                right.anchoredPosition = new Vector2(left.anchoredPosition.x - layer.width, right.anchoredPosition.y);
                // Reordena
                layer.parts[0] = right;
                layer.parts[1] = left;
            }
        }
    }
}
