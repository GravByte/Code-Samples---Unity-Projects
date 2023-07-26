using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField]
    bool _showLogs;

    [SerializeField]
    string _Prefix;

    //----------- Private variables
    private Transform lookAt;
    private Vector3 startOffset;
    private Vector3 moveVector;

    private float transition = 0.0f;
    private readonly float animationDuration = 3.0f;
    private Vector3 animationOffset = new Vector3 (0, -3, 4);

    //----------- Public&Serialised Variables
    [Header("Camera")]
    [SerializeField]
    Camera _mainCamera;

    private void Awake()
    {
        //----------- get a reference to the main camera
        _mainCamera = Camera.main;

        //----------- make sure the camera is using the solid color background instead of skybox
        _mainCamera.clearFlags = CameraClearFlags.Skybox;
    }

    void Start()
    {
        //----------- Finds player gameobject and centres camera position to it
        lookAt = GameObject.FindGameObjectWithTag("Player").transform;
        startOffset = transform.position - lookAt.position;
    }

    //----------- Update is called once per frame
    void Update()
    {
        //----------- Camera follow player position and movement
        moveVector = lookAt.position + startOffset;
        moveVector.x = 0;
        moveVector.y = Mathf.Clamp(moveVector.y, 3, 6);

        if (transition > 1.0f)
        {
            transform.position = moveVector;
        }
        else
        {
            //----------- Camera animation at start
            transform.position = Vector3.Lerp(moveVector + animationOffset, moveVector, transition);
            transition += Time.deltaTime * 1 / animationDuration;
            transform.LookAt(lookAt.position + Vector3.up);
        }

        transform.position = moveVector;
    }

    //-------- Logging Control Method
    public void Log(object message)
    {
        if (_showLogs)
            Debug.Log(_Prefix + message);
    }

}
