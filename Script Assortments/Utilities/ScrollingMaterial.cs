using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingMaterial : MonoBehaviour
{
    [SerializeField] Vector2 scrollSpeed = new Vector2(0.5f, 0.5f);
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        Vector2 offset = Time.time * scrollSpeed;
        rend.material.mainTextureOffset = offset;
    }
}
