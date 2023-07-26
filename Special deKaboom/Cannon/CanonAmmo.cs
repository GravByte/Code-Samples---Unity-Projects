using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CanonAmmo : MonoBehaviour
{
    
    public static UnityEvent DestroyAmmoEvent = new UnityEvent();
    
    // Start is called before the first frame update
    void Start()
    {
        //---- Create a listener for the DestroyAmmoEvent
        DestroyAmmoEvent.AddListener(DestroyAmmo);
    }
    
    private void DestroyAmmo()
    {
        Destroy(gameObject);
    }
}
