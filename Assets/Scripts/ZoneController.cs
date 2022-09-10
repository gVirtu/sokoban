using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneController : MonoBehaviour
{
    GameObject currentBox;
    public AudioClip turnOnClip;
    public AudioClip turnOffClip;

    private AudioSource source;

    // Start is called before the first frame update
    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (currentBox == null && other.CompareTag("Pushable"))
        {
            currentBox = other.gameObject;

            PushBoxController otherScript = currentBox.GetComponent<PushBoxController>();

            if (otherScript != null)
            {
                otherScript.TurnOn(gameObject);
                source.PlayOneShot(turnOnClip);
                GameManager.Instance.CheckVictory();
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
                otherScript.TurnOff(gameObject);
                source.PlayOneShot(turnOffClip);
            }

            currentBox = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
