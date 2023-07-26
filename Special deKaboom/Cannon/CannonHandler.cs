using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonHandler : MonoBehaviour
{
    [Header("Bools")]
    private bool isCannonLoaded = false;
    
    private bool isCannonRotatingLeft = false;
    private bool isCannonRotatingRight = false;

    [Header("Cannon Rotation Clamp")]
    [SerializeField] private int minClamp = -10;
    [SerializeField] private int maxClamp = 15;
    
    [Header("Cannon Rotation Speed")]
    [SerializeField] private float rotationSpeed = 100f;

    private Transform m_CannonTransform;
    
    [Header("Projectile")]
    [SerializeField] private Transform projectilePoint;
    [SerializeField] private GameObject projectilePrefab;
    
    [Header("Cannon Particle Systems")]
    [SerializeField] private ParticleSystem cannonFireEffect;
    
    [Header("Ready Light")]
    [SerializeField] private GameObject readyLight;
    [SerializeField] private Material readyLightOn;
    [SerializeField] private Material readyLightOff;
    
    // Start is called before the first frame update
    void Start()
    {
        //---- Set the ready light gameobject material to the readyLightOff material
        readyLight.GetComponent<MeshRenderer>().material = readyLightOff;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isCannonRotatingLeft)
        {
            //print("Rotating left!");
        
            // Rotate the game object left on the Y axis whilst the button is held down
            transform.Rotate(Vector3.up * (rotationSpeed * Time.deltaTime));
    
            // Clamp the rotation between -10 and 10 degrees
            Vector3 currentRotation = transform.localRotation.eulerAngles;
            currentRotation.y = (currentRotation.y > 180) ? currentRotation.y - 360 : currentRotation.y;
            currentRotation.y = Mathf.Clamp(currentRotation.y, minClamp, maxClamp);
            transform.localRotation = Quaternion.Euler(currentRotation);
        }
        else if (isCannonRotatingRight)
        {
            //print("Rotating right!");
        
            // Rotate the game object right on the Y axis whilst the button is held down
            transform.Rotate(Vector3.down * (rotationSpeed * Time.deltaTime));
    
            // Clamp the rotation between -10 and 10 degrees
            Vector3 currentRotation = transform.localRotation.eulerAngles;
            currentRotation.y = (currentRotation.y > 180) ? currentRotation.y - 360 : currentRotation.y;
            currentRotation.y = Mathf.Clamp(currentRotation.y, minClamp, maxClamp);
            transform.localRotation = Quaternion.Euler(currentRotation);
        }
    }


    [ContextMenu("Fire Cannon")]
    public void FireCannon()
    {
        //---- When the cannon is fired, the cannon is unloaded
        if (isCannonLoaded)
        {
            Instantiate(projectilePrefab, projectilePoint.position, projectilePoint.rotation);
            
            isCannonLoaded = false;
            cannonFireEffect.Play();
            print("Cannon fired!");
            
            readyLight.GetComponent<MeshRenderer>().material = readyLightOff;
        }
        else
        {
            print("Cannon is not loaded!");
        }
    }

    [ContextMenu("Load Cannon")]
    public void LoadCannon()
    {
        if (!isCannonLoaded)
        {
            isCannonLoaded = true;
            print("Cannon loaded!");
            
            readyLight.GetComponent<MeshRenderer>().material = readyLightOn;
        }
        else
        {
            print("Cannon is already loaded!");
        }
    }

    //---- When button is held down, changes the bool to true so that cannon rotates
    [ContextMenu("Rotate Cannon Left")]
    public void RotateCannonLeft() => isCannonRotatingLeft = true;
    [ContextMenu("Rotate Cannon Right")]
    public void RotateCannonRight() => isCannonRotatingRight = true;
    
    //---- When button is released, changes the bool to false so that cannon stops rotating
    [ContextMenu("Stop Rotating Cannon Left")]
    public void StopRotatingCannonLeft() => isCannonRotatingLeft = false;
    [ContextMenu("Stop Rotating Cannon Right")]
    public void StopRotatingCannonRight() => isCannonRotatingRight = false;


    private void OnTriggerEnter(Collider other)
    {
        //---- When cannon ammo is inserted into the cannon loader, the cannon is loaded and the ammo is destroyed
        if (other.gameObject.tag.Equals("Cannon Ammo"))
        {
            if (isCannonLoaded) return;
            LoadCannon();
            Destroy(other.gameObject);
        }
    }

}
