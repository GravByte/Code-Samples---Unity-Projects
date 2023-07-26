using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawManager : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField]
    bool _showLogs;

    [SerializeField]
    string _Prefix;

    private Transform followPlayer;
    private Vector3 startOffset;
    private Vector3 moveVector;

    // Start is called before the first frame update
    void Start()
    {
        //Disable logger if  not debug build
        Debug.unityLogger.logEnabled = Debug.isDebugBuild;

        //------- Get player transforms
        followPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        startOffset = transform.position - followPlayer.position;
    }

    // Update is called once per frame
    void Update()
    {
        //--------- Follow player transforms
        moveVector = followPlayer.position + startOffset;
        //moveVector.x = 0;

        transform.position = moveVector;
    }

    //-------- Logging Control Method
    private void Log(object message)
    {
        if (_showLogs)
            Debug.Log(_Prefix + message);
    }
}
