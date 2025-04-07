using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{


    private int points;
    private TextMeshProUGUI textMesh;






    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        textMesh.text = "Score: " + points.ToString();
    }


    public void AddPoints(int newPoints)
    {
        points += newPoints;
    }

    public void ResetPoints()
    {
        points = 0;
    }




}
