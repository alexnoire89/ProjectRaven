using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBarUI : MonoBehaviour
{

    private Slider slider;



    private void Start()
    {
        //inicializa slider
        slider = GetComponent<Slider>();
        initLifeBar(100);
    }

    public void ChangeMaxHP(int maxHP)
    {
        slider.maxValue = maxHP;
    }

    public void ChangeActualHP(int HPvalue)
    {
        slider.value = HPvalue;
    }

    public void initLifeBar(int HPvalue)
    {
        ChangeMaxHP(HPvalue);
        ChangeActualHP(HPvalue);
    }

}
