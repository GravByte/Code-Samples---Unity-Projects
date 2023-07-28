using System;
using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    
    [SerializeField] private float dragSpeed = 2f;
    
    [Header("Finger positions")]
    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;
    private Vector2 fingerDragDelta;

    [Header("Axis Clamps")]
    [SerializeField] private float xMin = -12f;
    [SerializeField] private float xMax = -5f;
    [SerializeField] private float zMin = -12f;
    [SerializeField] private float zMax = -5f;
    
    private bool isDragging = false;

    private void Update()
    {
        
        //---- Detect touch input
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (UIManager.Instance.isUIPanelOpen)
            {
                return;
            }
            
            switch (touch.phase)
            {
                //---- Detect finger down position to begin dragging
                case TouchPhase.Began:
                    fingerDownPosition = touch.position;
                    isDragging = true;
                    break;
                //---- Detect finger up position to stop dragging
                case TouchPhase.Ended:
                    fingerUpPosition = touch.position;
                    isDragging = false;
                    break;
            }

            //---- Calculate finger drag delta
            if (isDragging)
            {
                fingerDragDelta = touch.position - fingerDownPosition;

                // Calculate camera translation based on finger drag delta and drag speed
                float moveX = fingerDragDelta.x / Screen.width * dragSpeed;
                float moveZ = fingerDragDelta.y / Screen.height * dragSpeed;

                // Clamp camera translation to limits
                var position = transform.position;
                float newX = Mathf.Clamp(position.x + moveX, xMin, xMax);
                float newY = Mathf.Clamp(position.y, 10f, 10f);
                float newZ = Mathf.Clamp(position.z + moveZ, zMin, zMax);

                // Set new camera position
                position = new Vector3(newX, newY, newZ);
                transform.position = position;
            }
        }
    }
}