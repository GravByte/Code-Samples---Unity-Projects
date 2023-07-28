using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonEventHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("UI Elements")]
    [SerializeField] private Button button;
    
    [Header("Button States")]
    public bool isButtonPressed = false;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (button.interactable)
        {
            isButtonPressed = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (button.interactable)
        {
            isButtonPressed = false;
        }
    }
    
}

