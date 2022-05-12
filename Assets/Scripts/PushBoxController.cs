using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBoxController : MonoBehaviour
{
    public Material litMaterial;
    private Material originalMaterial;
    private bool isLit;
    private Renderer myRenderer;

    // Start is called before the first frame update
    void Start()
    {
        myRenderer = GetComponent<Renderer>();
        originalMaterial = myRenderer.sharedMaterial;
        isLit = false;
    }

    public void turnOn()
    {
        myRenderer.sharedMaterial = litMaterial;
        isLit = true;
    }

    public void turnOff()
    {
        myRenderer.sharedMaterial = originalMaterial;
        isLit = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
