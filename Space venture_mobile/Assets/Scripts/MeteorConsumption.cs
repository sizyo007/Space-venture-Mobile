using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MeteorConsumption : MonoBehaviour
{
    [Header("Consumption Settings")]
    [SerializeField] private int maxMeteoroidConsumption = 5;
    private int meteoroidConsumptionCount = 0;
    public int onConsumption;
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private ParticleSystem consumeEffect;
    [SerializeField] private GameObject consumePlusimage;
    private void Start()
    {
        consumePlusimage.SetActive(false);
    }
    public void Consumed()
    {
        meteoroidConsumptionCount++;
       
        if (meteoroidConsumptionCount >= maxMeteoroidConsumption)
        {
         
            meteoroidConsumptionCount = 0; // Reset count after launching
        }
    }

    private int _consumeCount;
    public void MeteorConsumed(int Consumption)
    {
        
        consumeEffect.Play();
        _consumeCount++;
        FindAnyObjectByType<AudioManager>().Play("Consume");
        
        StartCoroutine(ConsumePlus());
        if (_consumeCount >= Consumption)
        {
            
            FindAnyObjectByType<PlayerFireSystem>().IncreaseLaserCapacity();
            _consumeCount = 0;
           
           
        }
       
        // Logic for when a meteoroid is consumed can go here
    }
   public IEnumerator ConsumePlus()
    {
        //consumePlusimage.transform.position = Consumer.transform.position;

        consumePlusimage.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        consumePlusimage.SetActive(false);
       
      
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Metroid"))
        {
            if (healthSystem != null)
            {
                //Debug.Log("CONSUMPTION CALLED !!");
                other.GetComponent<Collider2D>().enabled = false;
                other.GetComponent<Meteoroid>().StopMovement();
                other.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(
                    () => { other.gameObject.SetActive(false); });
                MeteorConsumed(onConsumption);
            }
        }
    }

}
