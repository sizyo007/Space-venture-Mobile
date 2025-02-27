using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 3; // Player max health
    [HideInInspector] public int currentHealth; // To store the current health of the player

    private GameManager gameManager;
    [SerializeField] private PlayerFireSystem playerFireSystem;
    
    private Collider2D parentCollider;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        parentCollider = GetComponent<CircleCollider2D>();
        ResetHealth();
    }
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }
    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return;
        
        currentHealth -= damage;
        UpdateHealthUI();
        if (currentHealth >= 1)
        {
            FindAnyObjectByType<AudioManager>().Play("HealthMinus");
        }
        if (currentHealth == 0)
        {
            gameManager.GameOver();
            FindAnyObjectByType<AudioManager>().Play("GameOver");
            // Handle game over logic (optional)
            
        }
    }
    private void UpdateHealthUI()
    {
        gameManager.UpdateHealthUI(currentHealth);
    } 
}
