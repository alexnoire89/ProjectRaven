using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Lifes : MonoBehaviour
{
    private int lifes;
    private TextMeshProUGUI textMesh;

  

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        lifes = 3;
    }

    void Update()
    {
        textMesh.text = "Lifes: " + lifes.ToString();
    }


    public void AddLifes(int newLife)
    {
        lifes += newLife;
    }

    public void RemoveLifes(int newLife)
    {
        lifes -= newLife;
    }

    public void ResetLifes(int newLife)
    {
        lifes = newLife;
    }




}
