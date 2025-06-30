using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UIScrollLayer
{
    public RectTransform[] parts;      
    public float speed = 100f;         
    public Vector2 direction = Vector2.left;

    [Header("Efecto de Color")]
    public bool enableGradient = false;       
    public Color[] gradientColors;            
    public float colorCycleDuration = 2f;    

    [HideInInspector] public float width;
    [HideInInspector] public float colorTimer;
    [HideInInspector] public int currentColorIndex;
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
                layer.width = layer.parts[0].rect.width;

                //Posiciona lado a lado
                layer.parts[0].anchoredPosition = Vector2.zero;
                layer.parts[1].anchoredPosition = new Vector2(layer.width, 0f);
            }

            layer.colorTimer = 0f;
            layer.currentColorIndex = 0;
        }
    }

    void Update()
    {
        float deltaTime = Time.deltaTime;

        foreach (var layer in layers)
        {
            //Movimiento parallax
            foreach (RectTransform part in layer.parts)
            {
                part.anchoredPosition += layer.direction.normalized * layer.speed * deltaTime;
            }

            RectTransform left = layer.parts[0];
            RectTransform right = layer.parts[1];

            if (layer.direction.x < 0 && left.anchoredPosition.x <= -layer.width)
            {
                left.anchoredPosition = new Vector2(right.anchoredPosition.x + layer.width, left.anchoredPosition.y);
                //Reordena
                layer.parts[0] = right;
                layer.parts[1] = left;
            }
            else if (layer.direction.x > 0 && right.anchoredPosition.x >= layer.width)
            {
                right.anchoredPosition = new Vector2(left.anchoredPosition.x - layer.width, right.anchoredPosition.y);
                layer.parts[0] = right;
                layer.parts[1] = left;
            }


            //Efecto gradiente
            if (layer.enableGradient && layer.gradientColors.Length > 1)
            {
                layer.colorTimer += deltaTime;
                float t = (layer.colorTimer % layer.colorCycleDuration) / layer.colorCycleDuration;

                int fromIndex = layer.currentColorIndex;
                int toIndex = (fromIndex + 1) % layer.gradientColors.Length;

                Color from = layer.gradientColors[fromIndex];
                Color to = layer.gradientColors[toIndex];
                Color lerped = Color.Lerp(from, to, t);

                foreach (RectTransform part in layer.parts)
                {
                    Image img = part.GetComponent<Image>();
                    if (img != null) img.color = lerped;
                }

                //Cambiar indice de color cuando se completa un ciclo
                if (layer.colorTimer >= layer.colorCycleDuration)
                {
                    layer.colorTimer = 0f;
                    layer.currentColorIndex = toIndex;
                }
            }
        }
    }
}
