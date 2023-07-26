using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    
    [Header("Health")]
    [SerializeField] private float maxHealth = 40f;
    [SerializeField] private float currentHealth;
    [SerializeField] private Slider healthSlider;
    
    public bool godMode = false;
    
    [Header("Events")]
    public UnityEvent noHealthEvent = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        if (healthSlider != null)
            healthSlider.maxValue = maxHealth;

        if (godMode) print("God mode enabled!");
    }

    public void TakeDamage(float damage)
    {
        if (godMode) return;
        currentHealth -= damage;
        UpdateHealthUI(currentHealth);
        
        if (currentHealth <= 0)
        {
            noHealthEvent.Invoke();
            RemoveHealthUI();
        }
    }
    
    private void UpdateHealthUI(float health)
    {
        if (healthSlider != null)
            healthSlider.value = health;
    }
    
    private void RemoveHealthUI()
    {
        if (healthSlider != null)
            healthSlider.gameObject.SetActive(false);
    }
    
}
