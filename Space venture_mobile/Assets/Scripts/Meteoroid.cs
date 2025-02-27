using System.Collections;
using UnityEngine;

public enum MeteoroidType
{
    Straight,
    Curve,
    Complex,
    Zigzag,
    Spiral,
    Drifting,
    Bouncing,
    RandomDirectionChange
}

public class Meteoroid : MonoBehaviour
{
   
    private Transform _player;
    private float _speed;
    private MeteoroidType _type;

    private float _curveIntensity;
    private float _frequency;
    private float _amplitude;
    private float _time;

    [SerializeField] private GameObject visual;
    [SerializeField] private GameObject blastVisual;
    [SerializeField] private GameObject glowVisual;

    private bool _canMove = true;
    [SerializeField] private ParticleSystem laserHitEffect;

    private float _speedChange;

  

    public void Initialize(Transform playerTransform, float meteoroidSpeed, MeteoroidType meteoroidType, float intensity = 0, float freq = 0, float amp = 0)
    {
        _player = playerTransform;
        _speed = meteoroidSpeed;
        _type = meteoroidType;

        _curveIntensity = intensity;
        _frequency = freq;
        _amplitude = amp;

        _time = 0f; // Reset time for any time-based patterns
    }
    private void Update()
    {
        if (!_canMove) return;

        if (_player == null) return;
        _time += Time.deltaTime;

        switch (_type)
        {
            case MeteoroidType.Straight:
                MoveInStraightPath();
                break;
            case MeteoroidType.Curve:
                MoveInCurvePath();
                break;
            case MeteoroidType.Complex:
                MoveInComplexPath();
                break;
            case MeteoroidType.Zigzag:
                MoveInZigzagPath();
                break;
            case MeteoroidType.Spiral:
                MoveInSpiralPath();
                break;
            case MeteoroidType.Drifting:
                MoveInDriftingPath();
                break;
            case MeteoroidType.Bouncing:
                MoveInBouncingPath();
                break;
            case MeteoroidType.RandomDirectionChange:
                MoveInRandomDirection();
                break;
        }
    }
     public void Explode()
    {
        _canMove = false;
        GetComponent<Collider2D>().enabled = false;
        visual.SetActive(false);
        blastVisual.SetActive(true);
        glowVisual.SetActive(true);
        StartCoroutine(Disappear());
    }
    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(0.15f);
        glowVisual.SetActive(false);

        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }
    private void MoveInStraightPath()
    {
        Vector2 direction = MeteoroidManager.instance.endPosDir;
        transform.position += (Vector3)direction * _speed * Time.deltaTime;
    }
    private void MoveInCurvePath()
    { 
        Vector2 direction = MeteoroidManager.instance.endPosDir;
        Vector2 perpendicular = new Vector2(-direction.y, direction.x);
        Vector2 movement = direction + perpendicular * Mathf.Sin(_time * _curveIntensity);

        transform.position += (Vector3)movement * _speed * Time.deltaTime ;
    }
    private void MoveInComplexPath()
    {
        Vector2 direction = MeteoroidManager.instance.endPosDir;
        Vector2 wavePattern = new Vector2(Mathf.Sin(_time * _frequency) * _amplitude, Mathf.Cos(_time * _frequency) * _amplitude);
        Vector2 movement = direction + wavePattern;

        transform.position += (Vector3)movement.normalized * _speed * Time.deltaTime;
    }
    private void MoveInZigzagPath()
    {
        Vector2 direction = MeteoroidManager.instance.endPosDir;
        Vector2 zigzag = new Vector2(Mathf.Sin(_time * _frequency) * _amplitude, direction.y);
        transform.position += (Vector3)(direction + zigzag).normalized * _speed * Time.deltaTime;
    }
    private void MoveInSpiralPath()
    {
        Vector2 direction = MeteoroidManager.instance.endPosDir;
        float spiralX = Mathf.Cos(_time * _frequency) * _amplitude;
        float spiralY = Mathf.Sin(_time * _frequency) * _amplitude;
        Vector2 spiralMovement = direction + new Vector2(spiralX, spiralY);
        transform.position += (Vector3)spiralMovement.normalized * _speed * Time.deltaTime;
    }
    private void MoveInDriftingPath()
    {
        Vector2 randomDrift = new Vector2(Mathf.PerlinNoise(_time, 0) - 0.5f, Mathf.PerlinNoise(0, _time) - 0.5f);
        transform.position += (Vector3)randomDrift.normalized * _speed * Time.deltaTime;
    }
    private void MoveInBouncingPath()
    {
        Vector2 direction = MeteoroidManager.instance.endPosDir;
        Vector2 bounce = new Vector2(direction.x, Mathf.Abs(Mathf.Sin(_time * _frequency)) * _amplitude);
        transform.position += (Vector3)bounce.normalized * _speed * Time.deltaTime;
    }
    private void MoveInRandomDirection()
    {
        Vector2 randomChange = new Vector2(Mathf.Cos(_time * _frequency) * _amplitude, Mathf.Sin(_time * _frequency) * _amplitude);
        transform.position += (Vector3)randomChange.normalized * _speed * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<Collider2D>().enabled = false;
            other.GetComponent<HealthSystem>().TakeDamage(1);
            GameManager.instance.ShakeCamera();
        }
        if (other.CompareTag("laser"))
            { laserHitEffect.Play(); 
        }
    }
    public void StopMovement()
    {
        _canMove = false;
        visual.SetActive(false);
    }
}
