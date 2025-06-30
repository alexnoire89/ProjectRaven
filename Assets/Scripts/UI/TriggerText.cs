using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TriggerText : MonoBehaviour
{
    [SerializeField] private GameObject textTip;
    [SerializeField] private TextMeshProUGUI tipText;

    [Header("Texto del Tip")]
    [TextArea(2, 4)]
    public string message;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            textTip.SetActive(true);
            tipText.text = message;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            textTip.SetActive(false);
        }
    }
}
