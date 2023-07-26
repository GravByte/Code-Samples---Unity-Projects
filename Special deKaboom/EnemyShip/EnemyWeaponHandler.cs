using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyWeaponHandler : MonoBehaviour
{

    [SerializeField] private GameObject[] enemyLaunchers;
    private int _remainingLauncherCount;
    
    [SerializeField] private float minTimeBetweenMissiles = 2f;
    [SerializeField] private float maxTimeBetweenMissiles = 5f;
    
    public static UnityEvent AllLaunchersDestroyedEvent = new UnityEvent();
    
    // Start is called before the first frame update
    void Start()
    {
        enemyLaunchers = GameObject.FindGameObjectsWithTag("Enemy Launcher");
        _remainingLauncherCount = enemyLaunchers.Length;
        
        foreach (var enemyLauncher in enemyLaunchers)
        {
            enemyLauncher.GetComponent<Health>().noHealthEvent.AddListener(OnNoHealth);
        }
        
        FireMissiles();
    }

    //---- Fire missiles from each launcher in a random order at a random interval
    [ContextMenu("Fire Missiles")]
    public void FireMissiles()
    {
        StartCoroutine(FireMissilesCoroutine());
        print("Missile firing started");
    }
    
    [ContextMenu("Stop Firing Missiles")]
    public void StopFiringMissiles()
    {
        StopAllCoroutines();
        print("Missile firing stopped");
    }

    private IEnumerator FireMissilesCoroutine()
    {
        //---- From the list of enemyLaunchers pick a random launcher and fire a missile
        while (_remainingLauncherCount > 0)
        {
            var randomLauncher = enemyLaunchers[Random.Range(0, enemyLaunchers.Length)];
            randomLauncher.GetComponent<EnemyLauncher>().FireEnemyMissile();
            yield return new WaitForSeconds(Random.Range(minTimeBetweenMissiles, maxTimeBetweenMissiles));
        }
    }
    
    //---- If all the launchers are destroyed, stop firing missiles
    private void OnNoHealth()
    {
        _remainingLauncherCount--;
        print("Remaining launchers: " + _remainingLauncherCount);
    }
    
    private void Update()
    {
        if (_remainingLauncherCount <= 0)
        {
            StopFiringMissiles();
            AllLaunchersDestroyedEvent.Invoke();
            print("All launchers destroyed!");
        }
    }
}
