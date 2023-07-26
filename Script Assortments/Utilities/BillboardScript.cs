using UnityEngine;

namespace _Project.Scripts.VFX_Scripts
{
    public class BillboardScript : MonoBehaviour
    {

        private Camera _mCamera;

        // Start is called before the first frame update
        void Start()
        {
            _mCamera = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            transform.LookAt(_mCamera.transform.position, Vector3.up); 
        }
    }
}
