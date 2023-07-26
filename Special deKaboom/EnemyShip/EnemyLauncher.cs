using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyLauncher : MonoBehaviour
{

    [SerializeField] private Transform projectilePoint;
    [SerializeField] private GameObject projectilePrefab;
    
    [SerializeField] private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    // Start is called before the first frame update
    void Start()
    {
        health.noHealthEvent.AddListener(OnNoHealth);
    }


    [ContextMenu("Fire Enemy Missile")]
    public void FireEnemyMissile()
    {
        var missile = Instantiate(projectilePrefab, projectilePoint.position, projectilePoint.rotation);
        missile.GetComponent<MissileProjectile>().isFriendly = false;
        
        print("Enemy missile fired!");
    }
    
    private void OnNoHealth()
    {
        print("Enemy Launcher destroyed!");
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }
    
}
