using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBoxController : MonoBehaviour
{
    public Material litMaterial;
    private Material originalMaterial;
    private bool isLit;
    private Renderer myRenderer;
    private GameObject currentZone;

    // Start is called before the first frame update
    void Awake()
    {
        myRenderer = GetComponent<Renderer>();
        originalMaterial = myRenderer.sharedMaterial;
        isLit = false;
    }

    public void TurnOn(GameObject zone)
    {
        currentZone = zone;
        myRenderer.sharedMaterial = litMaterial;
        isLit = true;
    }

    public void TurnOff(GameObject zone)
    {
        if (currentZone == zone)
        {
            myRenderer.sharedMaterial = originalMaterial;
            isLit = false;
            currentZone = null;
        }
    }

    public bool IsLit()
    {
        return isLit;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
