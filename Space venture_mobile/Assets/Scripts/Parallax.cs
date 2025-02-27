using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] GameObject background;
    [SerializeField] GameObject StarField;
    [SerializeField] public float StaranimationSpeed = 1f;
    [SerializeField] public float animationSpeed = 1f;

    private MeshRenderer m_Renderer;
    private MeshRenderer s_Renderer;
    private void Awake()
    {
        m_Renderer = background.GetComponent<MeshRenderer>();
        s_Renderer = StarField.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Renderer != null)
        {
            Vector2 backgroundOffset = m_Renderer.material.mainTextureOffset;
            backgroundOffset += new Vector2(0, animationSpeed * Time.deltaTime);
            m_Renderer.material.mainTextureOffset = backgroundOffset;
        }

        // Update the offset for star field
        if (s_Renderer != null)
        {
            Vector2 starOffset = s_Renderer.material.mainTextureOffset;
            starOffset += new Vector2(0, StaranimationSpeed * Time.deltaTime);
            s_Renderer.material.mainTextureOffset = starOffset;
        }
    }
}
