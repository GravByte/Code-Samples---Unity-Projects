using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Harvester : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [Header("UI Elements")]
    [SerializeField] private Collider2D harvesterCollider;
    [SerializeField] private Button button;

    [Header("Sprites")] 
    private Image image;
    [SerializeField] private Sprite upSprite;
    [SerializeField] private Sprite downSprite;
    [SerializeField] private Sprite leftSprite;
    [SerializeField] private Sprite rightSprite;
    
    [Header("Button States")]
    public bool isButtonPressed = false;
    
    [Header("Vectors")]
    public Vector2 _harvesterStartPos;
    
    [SerializeField] private RectTransform _rectTransform;
    
    [SerializeField] private HarvestedResourceTask _harvestedResourceTask;
    
    private void Awake()
    {
        harvesterCollider = GetComponent<Collider2D>();
        button = GetComponent<Button>();

        image = GetComponent<Image>();
        
        _rectTransform = gameObject.GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //---- if the harvester is on cooldown, pressing the button will do nothing
        if (_harvestedResourceTask._isCooldown) return;
        
        //---- set the harvester's start position when the button is pressed
        _harvesterStartPos = transform.position;
        
        //---- set the button to interactable
        if (button.interactable)
        {
            isButtonPressed = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_harvestedResourceTask._isCooldown) return;

        if (isButtonPressed == false) return;

        Vector2 deltaPos = eventData.delta;

        //---- update image rect transform z based on movement direction
        if (Mathf.Abs(deltaPos.x) > Mathf.Abs(deltaPos.y))
        {
            //---- move left or right
            _rectTransform.rotation = Quaternion.Euler(0, 0, deltaPos.x > 0 ? 180 : 0);
        }
        else if (Mathf.Abs(deltaPos.y) > Mathf.Abs(deltaPos.x))
        {
            //---- move up or down
            _rectTransform.rotation = Quaternion.Euler(0, 0, deltaPos.y > 0 ? 90 : 270);
        }
        else
        {
            _rectTransform.rotation = deltaPos.x switch
            {
                //---- move diagonally
                > 0 when deltaPos.y > 0 => Quaternion.Euler(0, 0, 135),
                > 0 when deltaPos.y < 0 => Quaternion.Euler(0, 0, 225),
                < 0 when deltaPos.y > 0 => Quaternion.Euler(0, 0, 45),
                < 0 when deltaPos.y < 0 => Quaternion.Euler(0, 0, 315),
                _ => _rectTransform.rotation
            };
        }

        //---- move the harvester
        transform.position += (Vector3)deltaPos;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_harvestedResourceTask._isCooldown) return;
        
        transform.position = _harvesterStartPos;
        
        var rectTransformRotation = _rectTransform.rotation;
        rectTransformRotation.z = 720; //---- reset to default sprite
        
        if (button.interactable)
        {
            isButtonPressed = false;
            button.interactable = false;
            StartCoroutine(_harvestedResourceTask.Cooldown(_harvestedResourceTask.cooldownTimer));
        }
    }
    
    
    
}
