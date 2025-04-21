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



    public void OnPointerDown(PointerEventData eventData)
    {


        //background.gameObject.SetActive(true);
        //handle.gameObject.SetActive(true);
        //background.position = eventData.position;

        startPosition = eventData.position;
        handle.anchoredPosition = Vector2.zero;
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
        //background.gameObject.SetActive(false);
        //handle.gameObject.SetActive(false);

        handle.anchoredPosition = Vector2.zero;
        inputVector = Vector2.zero;
    }
}

