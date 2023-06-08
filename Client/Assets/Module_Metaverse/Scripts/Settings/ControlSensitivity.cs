using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlSensitivity : MonoBehaviour
{
    public Slider slider;
    public float sliderValue;
    public float sensi;
    
    // Start is called before the first frame update
    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("Sensitivity", 1f);
        sensi = slider.value;
        
    }
    /// <summary>
    /// Change the sensitivity
    /// </summary>
    /// <param name="value"></param>
    public void ChangeSlider(float value)
    {
        sliderValue = value;
        PlayerPrefs.SetFloat("Sensitivity", sliderValue);
        sensi = slider.value;
        //Debug.Log(sensi);
    }
}