using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneController : MonoBehaviour
{
    GameObject currentBox;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (currentBox == null && other.CompareTag("Pushable"))
        {
            currentBox = other.gameObject;

            PushBoxController otherScript = currentBox.GetComponent<PushBoxController>();

            if (otherScript != null)
            {
                otherScript.turnOn(gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentBox)
        {
            PushBoxController otherScript = currentBox.GetComponent<PushBoxController>();

            if (otherScript != null)
            {
                otherScript.turnOff(gameObject);
            }

            currentBox = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
