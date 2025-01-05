using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    private Slider slider;
    private float fillSpeed = 0.8f;
    public float targetValue = 0;

    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
    }

    void Start()
    {
        //IncrementValue(0.33f);
    }

    void Update()
    {
        float distance = Mathf.Abs(slider.value - targetValue);

        if (distance < 0.01f)
        {
            slider.value = targetValue; // Snap to targetValue
        }
        else
        {
            float speed = fillSpeed * (distance + 0.1f); // Add 0.1f to avoid zero multiplication
            slider.value = Mathf.MoveTowards(slider.value, targetValue, speed * Time.deltaTime);
        }
    }

    public void IncrementValue(float percentGainAmount)
    {
        targetValue = slider.value + percentGainAmount;
    }
}