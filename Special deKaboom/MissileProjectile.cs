using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class MissileProjectile : MonoBehaviour
{
    [Header("AI Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float turnSpeed = 5f;
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float damAmount = 1f;
    //[SerializeField] private float destroyTime = 15f;
    [Header("References")]
    public bool isFriendly = false;
    [SerializeField] private bool isSpecialPayload = false;
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject[] friendlyTargets;
    [SerializeField] private GameObject[] enemyTargets;
    [SerializeField] private GameObject explosionEffect;
    private Rigidbody _rb;
    
    public static UnityEvent missileDeathEvent = new UnityEvent();

    void Start() {
        
        //---- Get a reference to the rigidbody
        _rb = GetComponent<Rigidbody>();
        
        //---- Populate the friendly targets array with all the friendly targets in the scene
        friendlyTargets = GameObject.FindGameObjectsWithTag("Friendly Target");
        //---- Populate the enemy targets array with all the enemy targets in the scene
        enemyTargets = GameObject.FindGameObjectsWithTag("Enemy Launcher");

        //---- Get the targets array based on whether the missile is friendly or not
        if (!isSpecialPayload)
        {
            GameObject[] targets = isFriendly ? enemyTargets : friendlyTargets;
            target = targets[Random.Range(0, targets.Length)];
        }
        else
        {
            target = GameObject.FindGameObjectWithTag("Enemy Target");
        }
        

        //---- Play spawn particle effect
        //Instantiate(explosionEffect, transform.position, Quaternion.identity);

        //---- Destroy after destroyTime seconds
        //StartCoroutine(DestroyMissileCoroutine(destroyTime));
    }

    void FixedUpdate() {
        
        
        //---- Calculate direction towards player
        Vector3 directionToTarget = (target.transform.position - transform.position).normalized;

        //---- Smoothly turn towards player using Lerp
        var forward = transform.forward;
        Vector3 newDirection = Vector3.Lerp(forward, directionToTarget, turnSpeed * Time.fixedDeltaTime);
        _rb.MoveRotation(Quaternion.LookRotation(newDirection, transform.up)); //---- Rotate towards the target, but keep the same "up" vector

        

        //---- Detect obstacles using SphereCast
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, detectionRadius, transform.forward, out hit))
        {
            // Calculate new direction to move around obstacle
            newDirection = Vector3.Reflect(transform.forward, hit.normal);
            _rb.MoveRotation(Quaternion.LookRotation(newDirection, transform.up)); //---- Rotate towards the new direction
        }
        
        //---- Move towards player
        _rb.velocity = newDirection * (speed * Time.fixedDeltaTime);
    }

    //---- This is the code that will be used to detect when the missile collides with something
    void OnCollisionEnter(Collision other) {
        
        //---- If the AI collides with another collider, destroy it
        
        if (other.gameObject.GetComponent<Health>())
        {
                other.gameObject.GetComponent<Health>().TakeDamage(damAmount);
                MissileHurt();
        }
        else
        {
            MissileHurt();
        }
        

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Main Ship") && !isFriendly && !isSpecialPayload)
        {
            print("Main Ship hit!");

            //---- Deal damage to the main ship
            other.gameObject.GetComponent<Health>().TakeDamage(damAmount);
            
            //---- Kill this entity
            MissileHurt();

        }
    }

    public void MissileHurt() {
        //---- Play explosion particle effect and destroy entity
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
        
        //gameObject.SetActive(false);
        StartCoroutine(DestroyMissileCoroutine(0.1f));
    }

    private IEnumerator DestroyMissileCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        missileDeathEvent.Invoke();
        Destroy(gameObject);
    }

}
