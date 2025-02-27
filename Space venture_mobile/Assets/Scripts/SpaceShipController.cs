using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections;
public class SpaceShipController : MonoBehaviour

{
    [Header("Movement Settings")]
    [SerializeField] private float rotateSpeed = 100f;
  
    public float moveSpeedKeyboard = 500f;
    public float rotateSpeedKeyboard = 50f;
    [SerializeField] private Slider horizontalSlider;
    [SerializeField] private Slider verticalSlider;



    [Header("ConsumerReference")]
    [SerializeField]
    private float rotationSpeed = 300;
    public GameObject Consumer;

    [Header("References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private HealthSystem _playerHealth;
    [SerializeField] private GameObject player;
    private Rigidbody2D rb;

    private Camera _mainCamera;
    private float _screenHalfWidth;
    private  bool _isDragging = false;
    float _horizontalInput;
    float _verticalInput;

    [SerializeField] private GameObject playImage;
    private void Start()
    {
       
        InitializeSliders();

        _mainCamera = Camera.main;
        _screenHalfWidth = (_mainCamera.orthographicSize * _mainCamera.aspect) - 0.5f;

        rb=GetComponent<Rigidbody2D>();
       
    }

    private void InitializeSliders()
    {
        if (horizontalSlider != null)
        {
            horizontalSlider.minValue = -1;
            horizontalSlider.maxValue = 1;
            horizontalSlider.value = 0;
        }

        if (verticalSlider != null)
        {
            verticalSlider.minValue = -1;
            verticalSlider.maxValue = 1;
            verticalSlider.value = 0;

            EventTrigger trigger = verticalSlider.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entryPointerDown = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
            entryPointerDown.callback.AddListener((data) => { _isDragging = true; });
            trigger.triggers.Add(entryPointerDown);

            EventTrigger.Entry entryPointerUp = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
            entryPointerUp.callback.AddListener((data) => { OnVerticalSliderReleased(); });
            trigger.triggers.Add(entryPointerUp);
        }
    }
    private void Update()
    {
        if (_isDragging)
        {
            HandleSliderInput();
            HandleMovement();
        }
        /*
         // enabling KeyBoard Movment
        if (!_isDragging) { 
        HandleKeyInput();
        }*/
        ConsumerRotation();
    }
    private void HandleSliderInput()
    {
        Vector2 inputPos = Input.touchCount > 0 ? Input.GetTouch(0).position : (Input.GetMouseButton(0) ? (Vector2)Input.mousePosition : Vector2.zero);
       
        if (inputPos != Vector2.zero)
        {
            UpdateSliderValue(horizontalSlider, inputPos, RectTransform.Axis.Horizontal);
            UpdateSliderValue(verticalSlider, inputPos, RectTransform.Axis.Vertical);
        }
        else
        {
            _isDragging = false;  
        }
    }
    private void HandleMovement()
    {
        if (horizontalSlider != null)
        {
            Vector2 newPosition = transform.position;
            newPosition.x = Mathf.Lerp(-_screenHalfWidth, _screenHalfWidth, (horizontalSlider.value + 1) / 2);
            transform.position = newPosition;
        }

        if (verticalSlider != null && Mathf.Abs(verticalSlider.value) > 0.25f)
        {
            transform.Rotate(Vector3.forward, -verticalSlider.value * rotateSpeed * Time.deltaTime);
        }
    }/*
      //player Movment and rotation By KeyBoard 
   public void HandleKeyInput()
    {
        _verticalInput = Input.GetAxis("Vertical");
        _horizontalInput = Input.GetAxis("Horizontal");

        _isDragging = false;
        
        transform.Rotate(Vector3.forward * _verticalInput * rotateSpeedKeyboard * Time.fixedDeltaTime);//When _horizontalInput button is pressed player will rotate 180� in left and right

        rb.linearVelocity = new Vector2(_horizontalInput, rb.linearVelocity.y) * moveSpeedKeyboard * Time.fixedDeltaTime;//when _horizontalInput button is pressed player will mover left and right

        horizontalSlider.enabled = false;
        verticalSlider.enabled = false;

        if (player.transform.position.x< -2.3f)//Boundaries for player for not moving out of screen
        {
            player.transform.position = new Vector2(-2.3f, transform.position.y);
        }
        if (player.transform.position.x > 2.3f)
        {
            player.transform.position = new Vector2(2.3f, transform.position.y);
        }
        
    }
    */
    private void OnVerticalSliderReleased()
    {
        _isDragging = false;
        ResetSlider(verticalSlider);
    }

    private void UpdateSliderValue(Slider slider, Vector2 touchPosition, RectTransform.Axis axis)
    {
        if (slider == null) return;

        RectTransform sliderRect = slider.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(sliderRect, touchPosition, null, out Vector2 localPoint);

        float normalizedValue = Mathf.InverseLerp(
            axis == RectTransform.Axis.Horizontal ? sliderRect.rect.xMin : sliderRect.rect.yMin,
            axis == RectTransform.Axis.Horizontal ? sliderRect.rect.xMax : sliderRect.rect.yMax,
            axis == RectTransform.Axis.Horizontal ? localPoint.x : localPoint.y
        );

        slider.value = Mathf.Lerp(slider.minValue, slider.maxValue, normalizedValue);
    }

    private void ResetSlider(Slider slider)
    {
        if (slider != null)
        {
            slider.value = 0;
        }
    }
    public void DisableSlider()
    {
        horizontalSlider.enabled = false;
        verticalSlider.enabled = false;

        _isDragging = false;  
    }
   private void ConsumerRotation()
    {
        Consumer.transform.Rotate(new Vector3(0, 0, rotationSpeed) * Time.deltaTime);
    }
     
}
