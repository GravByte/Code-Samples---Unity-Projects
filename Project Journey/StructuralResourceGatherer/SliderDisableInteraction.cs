using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderDisableInteraction : MonoBehaviour, IPointerUpHandler
{
    
    [SerializeField] private Slider _slider;

    private float _sliderValue;
    
    private void Start()
    {
        //---- Get the slider component
        _slider = GetComponent<Slider>();
    }

    //---- Updates the slider value when changed and assigns to variable
    public void OnSliderValueChanged()
    {
        _sliderValue = _slider.value;
    }
    
    //---- When the mouse is released, check if the slider value is greater than 0, then disable the slider
    public void OnPointerUp(PointerEventData eventData)
    {
        if (_sliderValue > 0f)
        {
            _slider.interactable = false;
            //Debug.Log("Slider interaction disabled");
        }
    }

}
