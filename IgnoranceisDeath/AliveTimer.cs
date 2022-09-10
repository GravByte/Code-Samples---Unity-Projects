using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliveTimer : MonoBehaviour
{
    // Time in seconds that the monster is kept at bay 
    public int timeAlive = 0;
    public GameObject enemy;

    private void Start() {
        StartCoroutine(StartTimer());
    }

    private IEnumerator StartTimer() {

        // Increase the timer while the monster is not free
        while (enemy.GetComponent<EnemyManager>().isMonsterFree == false) {
            yield return new WaitForSeconds(1f);
            timeAlive++;
        }
    }
}
