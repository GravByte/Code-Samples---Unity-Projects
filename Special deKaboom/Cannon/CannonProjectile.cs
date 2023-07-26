using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonProjectile : MonoBehaviour
{

    [SerializeField] private float projSpeed = 10f;
    [SerializeField] private float projDamage = 10f;
    
    [SerializeField] private float destroyTime = 15f;
    
    private Rigidbody _rb;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        
        DestroyObject(destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        //---- the projectile moves forward at a constant speed
        transform.Translate(Vector3.forward * (Time.deltaTime * projSpeed));
    }
    
    private void OnCollisionEnter(Collision other)
    {
        print("Projectile collided with " + other.gameObject.name + "!");
        
        //---- when the projectile collides with an object, it checks if the object has a health component
        //---- if it does, it deals damage to the object
        //---- if it doesn't, it destroys the projectile
        if (other.gameObject.GetComponent<Health>())
        {
            other.gameObject.GetComponent<Health>().TakeDamage(projDamage);
            DestroyObject((0.1f));
        }
        else
        {
            DestroyObject(0.1f);
        }
    }
    
    private void DestroyObject(float time)
    {
        Destroy(gameObject, time);
    }
}
