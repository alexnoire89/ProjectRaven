using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIFacade : MonoBehaviour
{
    private int points;
    private int lifes;
    private Slider lifeBarSlider;
    private TextMeshProUGUI scoreAndLifesText;

    private void Start()
    {
        lifeBarSlider = GetComponentInChildren<Slider>();
        scoreAndLifesText = GetComponentInChildren<TextMeshProUGUI>();

        points = 0;
        lifes = 3;
        UpdateUI();
    }

   //Score
    public void AddPoints(int newPoints)
    {
        points += newPoints;
        UpdateUI();
    }

    public void ResetPoints()
    {
        points = 0;
        UpdateUI();
    }

    //Vidas
    public void AddLifes(int newLife)
    {
        lifes += newLife;
        UpdateUI();
    }

    public void RemoveLifes(int newLife)
    {
        lifes -= newLife;
        UpdateUI();
    }

    public void ResetLifes(int newLife)
    {
        lifes = newLife;
        UpdateUI();
    }

    //Salud
    public void ChangeMaxHP(int maxHP)
    {
        lifeBarSlider.maxValue = maxHP;
        lifeBarSlider.value = maxHP; 
    }

    public void ChangeActualHP(int HPvalue)
    {
        lifeBarSlider.value = HPvalue;
    }

    public void InitLifeBar(int HPvalue)
    {
        ChangeMaxHP(HPvalue);
        ChangeActualHP(HPvalue);
    }

    private void UpdateUI()
    {
        scoreAndLifesText.text = "Score: " + points.ToString() + "\nLifes: " + lifes.ToString();
    }
}
