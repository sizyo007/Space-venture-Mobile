using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StarsMovement : MonoBehaviour
{
    [SerializeField] public float animationSpeed = 1f;
    private SpriteRenderer spriteRenderer;
    private MaterialPropertyBlock materialPropertyBlock;
    private Vector2 initialOffset;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        materialPropertyBlock = new MaterialPropertyBlock();
        // Initialize the initial offset
        spriteRenderer.GetPropertyBlock(materialPropertyBlock);
        initialOffset = materialPropertyBlock.GetVector("_MainTex_ST");
    }

   /* private void Start()
    {
        spriteRenderer.GetPropertyBlock(materialPropertyBlock);
        initialOffset = materialPropertyBlock.GetVector("_MainTex_ST");
    }*/

    private void Update()
    {
     
        Vector2 offset = initialOffset + new Vector2(Time.time * animationSpeed % 1, 0); //wrap offset to stay within 0-1
        spriteRenderer.GetPropertyBlock(materialPropertyBlock);
       
        Debug.Log("working");
        materialPropertyBlock.SetVector("_MainTex_ST", offset);
        spriteRenderer.SetPropertyBlock(materialPropertyBlock);
    }
}
