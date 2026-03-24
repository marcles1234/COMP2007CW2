using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider slider;
    public float increaseRate;
    public float decreaseRate;
    public int increase; //0 = rest, 1 = decrease, 2 = increase

    void Start()
    {
        increase = 2;
    }

    void Update()
    {
        
        if (increase == 2)
        {
            slider.value += increaseRate * Time.deltaTime;
        }
        else if (increase == 1)
        {
            slider.value -= decreaseRate * Time.deltaTime;
        }
    }
}
