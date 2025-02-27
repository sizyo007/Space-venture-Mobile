using System;
using UnityEngine;

namespace ElementRelated
{
    public class LaserElement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float autoDestroyDelay = 3.5f;
        private float _duration;
      
        private void Update()
        {
            _duration += Time.deltaTime;
            if(_duration >= autoDestroyDelay) gameObject.SetActive(false);
            
            transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Metroid"))
            {
                
                
                GameManager.instance.AddScore(5);
                other.GetComponent<Meteoroid>().Explode();
                
                gameObject.SetActive(false);
                
            }
        }
    }
}
