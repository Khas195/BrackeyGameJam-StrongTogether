using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AISatisfaction : MonoBehaviour
{
    [SerializeField] Slider satisfactionSlider;

    private float currentSatisfaction = 50f;


    public void ChangeSatisfactionBy(float changeValue)
    {
        currentSatisfaction += changeValue;

        if(currentSatisfaction <= 0f)
        {
            currentSatisfaction = 0f;
        }

        if(currentSatisfaction >= 100f)
        {
            currentSatisfaction = 100f;
        }

        UpdateSatisfactionMeter();
    }

    private void UpdateSatisfactionMeter()
    {
        satisfactionSlider.value = currentSatisfaction;
    }
}
