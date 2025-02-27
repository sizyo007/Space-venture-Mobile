using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
  
    [Header("Health UI")]
    [SerializeField] private TextMeshProUGUI levelCompleteText;
    [SerializeField] private Image[] ships; // Array of ships image components

    private HealthSystem healthSystem; // Reference to the HealthSystem
    private PlayerFireSystem playerFireSystem;
    private SpaceShipController spaceShipController;
    private int score = 0;
    private int meteoroidConsumptionCount = 0;
 
    private Transform _camera;

    [Header("Cam Shake Effect")] [SerializeField]
    private float duration = 0.5f;
    [SerializeField] private float magnitude = 0.05f;
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else instance = this;
        
        healthSystem = FindAnyObjectByType<HealthSystem>();
        playerFireSystem = FindAnyObjectByType<PlayerFireSystem>();
        spaceShipController = FindAnyObjectByType<SpaceShipController>();

    }

    private void Start()
    {
        _camera = GameObject.FindWithTag("MainCamera").transform;
    }


    public void UpdateHealthUI(int currentHealth)
    {
        for (int i = 0; i < ships.Length; i++)
        {
            ships[i].enabled = i < currentHealth;
        }

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        FindAnyObjectByType<MenuManager>().EndGame();
        FindAnyObjectByType<MenuManager>().gameOverScoreText.text = score.ToString();
        Time.timeScale = 0;
        spaceShipController.DisableSlider();
    }

    public void AddScore(int points)
    {
        score += points;
        UiManager.instance.UpdateScoreUi(score);
       
    }

   
    

    public void ConsumeMeteoroid()
    {
        meteoroidConsumptionCount++;
        //healthSystem.MeteorConsumed(); // Call method in HealthSystem if needed
    }

    IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-0.1f, 0.1f) * magnitude;
            float offsetY = Random.Range(-0.1f, 0.1f) * magnitude;

            _camera.localPosition = new Vector3(originalPosition.x + offsetX, originalPosition.y + offsetY, -10);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
    }

    public void ShakeCamera()
    {
        StartCoroutine(Shake(duration, magnitude));
    }
    public void PauseButton()
    {
        FindAnyObjectByType<MenuManager>().Pause();
    }

   
}