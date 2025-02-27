using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerFireSystem : MonoBehaviour
{
    [Header("Laser Setting")]
    [SerializeField] float laserTime = 0.05f;
    [SerializeField] float _laserBeamLength = 10f; //laser beam length
    [SerializeField] private GameObject laserBeamPrefab;
    private int laserCapacity = 5;
   

    [Header("Reference")]
    private LineRenderer _lineRenderer; // Reference to the LineRenderer component
    private Coroutine _laserCoroutine; // Coroutine reference for laser management
    private LayerMask _layerMask;  // Layer mask for raycasting
    private GameManager _gameManager;
    private UiManager _uiManager;

    public SpaceShipController _spaceShipController;
    private float playerMoveDelay = 130f;
    private float playerRotateDelay = 7f;
   
    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _layerMask = LayerMask.GetMask("Metroid", "Enemy");  
    }
    

    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer.positionCount = 2;  // Set number of points for the line renderer
        _lineRenderer.enabled = false;   // Initially disable the laser

        
        UiManager.instance.UpdateLaserCountUi(laserCapacity);

       
        playerMoveDelay = _spaceShipController.moveSpeedKeyboard;
        playerRotateDelay = _spaceShipController.rotateSpeedKeyboard;
    }
    public void laserFireButton()
    {
        
            if (laserCapacity <= 5 && laserCapacity >= 1)
            {
                laserCapacity--;
                UiManager.instance.UpdateLaserCountUi(laserCapacity);
                FireLaserNew();
                StartCoroutine(StopMovment());
        }
        if (laserCapacity == 0)
        {
            //FindAnyObjectByType<AudioManager>().PauseAllGameSound();
        }

    }
        public void IncreaseLaserCapacity()
    {
        if (laserCapacity >= 5) return;
        laserCapacity++;
        UiManager.instance.UpdateLaserCountUi(laserCapacity);
    }
    
    
    public void FireLaserNew()
    {
            FindAnyObjectByType<AudioManager>().Play("laserFire");
            GameObject laser = Instantiate(laserBeamPrefab, transform.position, transform.rotation);
      
            Destroy(laser, 6);
    }
    public void FireLaser()
    {
        if(_laserCoroutine != null)
        {
            StopCoroutine(_laserCoroutine);
        }
        RaycastHit hit;
        Vector2 laserEndPos;

        //perform raycast to detect hits
        if(Physics.Raycast(transform.position, transform.up , out hit, _laserBeamLength , _layerMask))
        {
            laserEndPos = hit.point; //laser end position at the hit point

            if(hit.collider.CompareTag("Metroid"))
            { 
                Destroy(hit.collider.gameObject);
                _gameManager.AddScore(5);
            }
            else if(hit.collider.CompareTag("Enemy"))
            {
                Destroy(hit.collider.gameObject);
            }
        }
        else
        {
            laserEndPos = transform.position + transform.up * _laserBeamLength; //arbitrary long distance if no hit
            
        }

        _lineRenderer.SetPosition(0, transform.position); //start of laser
        _lineRenderer.SetPosition(1, laserEndPos); //end of laser

        _lineRenderer.enabled = true; //activate the laser
       
        _laserCoroutine = StartCoroutine(StopLaser());
    }

    private IEnumerator StopLaser()
    {
        yield return new WaitForSeconds(laserTime + Time.deltaTime);
        _lineRenderer.enabled = false;
        //Debug.Log("Laser is deactivated");
    }
    private void UpdateLaserPosition()
    {
        if (_lineRenderer.enabled)
        {

            Vector2 endPosition = transform.position + (transform.up * _laserBeamLength);
            _lineRenderer.SetPositions(new Vector3[] { transform.position, endPosition });
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if(_laserCoroutine != null)
        {
            StopCoroutine(_laserCoroutine);
        }
        StartCoroutine(StopLaser());
    }
   
   IEnumerator StopMovment()
    {
       
        _spaceShipController.rotateSpeedKeyboard-=playerRotateDelay;
        _spaceShipController.moveSpeedKeyboard-=playerMoveDelay;
        yield return new WaitForSeconds(0.008f);
        _spaceShipController.moveSpeedKeyboard +=playerMoveDelay;
        _spaceShipController.rotateSpeedKeyboard +=playerRotateDelay;
       
    }

}
