using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public RectTransform background;
    public RectTransform handle;
    public float handleRange = 100f;

    private Vector2 inputVector = Vector2.zero;
    private Vector2 startPosition;


    //get para direccion del joystick en playerController
    public Vector2 InputDirection => inputVector;

    void Start()
    {
        // Ocultar joystick al inicio
        background.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        Debug.Log("Presionooooooooooo");


        //startPosition = eventData.position;
        //handle.anchoredPosition = Vector2.zero;


        // Activamos el fondo del joystick
        background.gameObject.SetActive(true);

        // Mostrar y posicionar joystick donde se toca
        //background.position = eventData.position;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform as RectTransform, // Este debe ser el panel padre del joystick
            eventData.position,
            eventData.pressEventCamera,
            out localPoint
        );

        // Colocamos el joystick en esa posición
        background.anchoredPosition = localPoint;


        // Reiniciamos la palanca
        handle.anchoredPosition = Vector2.zero;

        // Guardamos esa posición como referencia para el drag
        startPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 moveDistance = eventData.position - startPosition;
        Vector2 clamped = Vector2.ClampMagnitude(moveDistance, handleRange);
        handle.anchoredPosition = clamped;

        inputVector = clamped / handleRange;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //handle.anchoredPosition = Vector2.zero;
        //inputVector = Vector2.zero;

        // Ocultar y resetear
        background.gameObject.SetActive(false);
        handle.anchoredPosition = Vector2.zero;
        inputVector = Vector2.zero;
    }
}

