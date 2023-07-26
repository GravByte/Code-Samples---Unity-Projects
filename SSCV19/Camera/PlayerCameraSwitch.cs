using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCameraSwitch : MonoBehaviour
{

    [SerializeField] CinemachineVirtualCamera _playerCam;
    [SerializeField] CinemachineVirtualCamera _lockCam;
    [SerializeField] CinemachineVirtualCamera _aimCam;

    // Start is called before the first frame update
    private void OnEnable()
    {
        CameraSwitcher.Register(_playerCam);
        CameraSwitcher.Register(_lockCam);
        CameraSwitcher.Register(_aimCam);
        CameraSwitcher.SwitchCamera(_playerCam);
    }

    private void OnDisable()
    {
        CameraSwitcher.Unregister(_playerCam);
        CameraSwitcher.Unregister(_lockCam);
        CameraSwitcher.Unregister(_aimCam);
    }

    // Update is called once per frame
    void Update()
    {
        //Switch Camera with keypress
        if (Input.GetKeyDown(KeyCode.L))
        {
            //Switch Camera
            if (CameraSwitcher.IsActiveCamera(_playerCam))
            {
                CameraSwitcher.SwitchCamera(_aimCam);
            }
            else if (CameraSwitcher.IsActiveCamera(_aimCam))
            {
                CameraSwitcher.SwitchCamera(_playerCam);
            }
        }
    }
}
