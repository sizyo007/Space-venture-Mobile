using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class MeteoroidManager : MonoBehaviour
{
    public static MeteoroidManager instance;

    [Header("Meteoroid Types")]
    [SerializeField] private GameObject _straightMeteoroid;

    [SerializeField] private GameObject _curveMeteoroid;
    [SerializeField] private float curveIntensity = 1f;

    [SerializeField] private GameObject _complexMeteoroid;
    [SerializeField] private float frequency = 2f;  // Frequency of the wave
    [SerializeField] private float amplitude = 0.9f;  // Amplitude of the wave

    [SerializeField] private GameObject _zigzagMeteoroid;
    [SerializeField] private GameObject _spiralMeteoroid;
    [SerializeField] private GameObject _driftingMeteoroid;
    [SerializeField] private GameObject _bouncingMeteoroid;
    [SerializeField] private GameObject _randomDirectionMeteoroid;


    [Header("Speed Randomization")]
    [SerializeField] private float slowSpeedMin = 0.3f;
    [SerializeField] private float slowSpeedMax = 0.5f;

    [SerializeField] private float mediumSpeedMin = 0.6f;
    [SerializeField] private float mediumSpeedMax = 0.8f;

    [SerializeField] private float fastSpeedMin =0.9f;
    [SerializeField] private float fastSpeedMax = 1f;
    [Header("Spawn Settings")]
    [SerializeField] private Transform[] spawnPoints; // Multiple spawn points
    [SerializeField] private float spawnInterval = 1f;

    private Transform _player;
   [HideInInspector] public bool _isPhase_1;

    [Header("Rigidbody settings")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Rigidbody2D _rb_1;
    [SerializeField] private Rigidbody2D _rb_2;

    private float rbSpeed = -0.0001f;
    private float rbSpeedIncreserOverTime ;

   
    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
       
        _isPhase_1 = true;
        _player = GameObject.FindWithTag("Player")?.transform;
        if (_player == null)
        {
            Debug.LogWarning("Player object not found. Meteoroid movement may be affected.");
            return;
        }

        // Start spawning from the third spawn point for the first 3-4 seconds
        StartCoroutine(ActivateThirdSpawnerForInitialSeconds());
        StartCoroutine(IncreseSpeedOverTime());
    }

    private IEnumerator ActivateThirdSpawnerForInitialSeconds()
    {
       
        // Spawn from the third point for 3-4 seconds
        float endTime = Time.time + Random.Range(14,15);

        while (Time.time < endTime)
        {
            yield return new WaitForSeconds(3f);
            _rb.AddForceY(rbSpeed);
            _rb_1.AddForceY(rbSpeed);
            _rb_2.AddForceY(rbSpeed);
            SpawnMeteoroidFromPoint(spawnPoints[2]);
            
        }
        // Start the regular spawning after that
        InvokeRepeating(nameof(SpawnMeteoroids), 2f, spawnInterval);
       
    }
    private void SpawnMeteoroids()
    {
        if (_player == null) return;

        // Randomly decide to spawn from one or more spawn points
        bool spawnFromFirst = Random.value > 0.7f;
        bool spawnFromSecond = Random.value > 0.7f;
        bool spawnFromThird = !spawnFromFirst && !spawnFromSecond; // Only spawn from the third if both others are false

        if (spawnFromFirst)
        {
            SpawnMeteoroidFromPoint(spawnPoints[0]);
            _isPhase_1 = false;
        }

        if (spawnFromSecond)
        {
            SpawnMeteoroidFromPoint(spawnPoints[1]);
            _isPhase_1 = false;
        }

        if (spawnFromThird)
        {
            // Fallback: spawn from the third point if the other two didn't spawn
            SpawnMeteoroidFromPoint(spawnPoints[2]);
        }
    }

    private void SpawnMeteoroidFromPoint(Transform spawnPoint)
    {
        MeteoroidType randomType = (MeteoroidType)Random.Range(0, Enum.GetValues(typeof(MeteoroidType)).Length);
        StartCoroutine(FlashMeteoroidAttackLine(randomType, spawnPoint));
    }
   [HideInInspector] public Vector3 endPosDir;
    private IEnumerator FlashMeteoroidAttackLine(MeteoroidType randomType, Transform spawnPoint)
    {
        if (randomType == MeteoroidType.Straight)
        {
            endPosDir = (_player.position + Vector3.right * Random.Range(-2f, 2f) - spawnPoint.position).normalized; 
        }

        GameObject meteoroidPrefab = GetMeteoroidPrefab(randomType);
        float speed = GetRandomizedMeteoroidSpeed();
      
        if (meteoroidPrefab != null )
        {
            GameObject meteoroidInstance = Instantiate(meteoroidPrefab, spawnPoint.position, Quaternion.identity);
            InitializeMeteoroid(meteoroidInstance, randomType, speed* rbSpeedIncreserOverTime);
            Destroy(meteoroidInstance, 6f);
            
        }

        yield return new WaitForSeconds(1f);
    }
    private GameObject GetMeteoroidPrefab(MeteoroidType type)
    {
        return type switch
        {
            MeteoroidType.Straight => _straightMeteoroid,
            MeteoroidType.Curve => _curveMeteoroid,
            MeteoroidType.Complex => _complexMeteoroid,
            MeteoroidType.Zigzag => _zigzagMeteoroid,
            MeteoroidType.Spiral => _spiralMeteoroid,
            MeteoroidType.Drifting => _driftingMeteoroid,
            MeteoroidType.Bouncing => _bouncingMeteoroid,
            MeteoroidType.RandomDirectionChange => _randomDirectionMeteoroid,
            _ => null,
        };
    }
   
    private float GetRandomizedMeteoroidSpeed()
    {
        // Randomize the speed: slow, medium, or fast
        int speedCategory = Random.Range(0, 3);

        return speedCategory switch
        {
            0 => Random.Range(slowSpeedMin, slowSpeedMax), // Slow speed
            1 => Random.Range(mediumSpeedMin, mediumSpeedMax), // Medium speed
            2 => Random.Range(fastSpeedMin, fastSpeedMax), // Fast speed
            _ => 0f, // Default case, shouldn't happen
        };
    }
    private void InitializeMeteoroid(GameObject meteoroidInstance, MeteoroidType type, float speed)
    {
        Meteoroid meteoroid = meteoroidInstance.GetComponent<Meteoroid>();
        switch (type)
        {
            case MeteoroidType.Straight:
                meteoroid.Initialize(_player, speed, type);
                break;
            case MeteoroidType.Curve:
                meteoroid.Initialize(_player, speed, type, curveIntensity);
                break;
            case MeteoroidType.Complex:
                meteoroid.Initialize(_player, speed, type, frequency, amplitude);
                break;
            case MeteoroidType.Zigzag:
                meteoroid.Initialize(_player, speed, type, frequency, amplitude);
                break;
            case MeteoroidType.Spiral:
                meteoroid.Initialize(_player, speed, type, frequency, amplitude);
                break;
            case MeteoroidType.Drifting:
                meteoroid.Initialize(_player, speed, type, curveIntensity);
                break;
            case MeteoroidType.Bouncing:
                meteoroid.Initialize(_player, speed, type, frequency);
                break;
            case MeteoroidType.RandomDirectionChange:
                meteoroid.Initialize(_player, speed, type);
                break;
        }
    }
    private IEnumerator IncreseSpeedOverTime()
    {

        while (Time.time > 0)
        {
            yield return new WaitForSeconds(10f);
            rbSpeedIncreserOverTime = rbSpeedIncreserOverTime + 0.5f;
            Debug.Log("Incresed");

        }
    }

}