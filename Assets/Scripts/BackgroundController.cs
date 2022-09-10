using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public Color baseColor;

    void Start()
    {
        float hue;
        float saturation;
        float value;

        Color.RGBToHSV(baseColor, out hue, out saturation, out value);

        hue -= (GameManager.Instance.GetSelectedLevelIndex() / 20f);
        saturation -= (GameManager.Instance.GetSelectedLevelIndex() / 160f);
        value -= (GameManager.Instance.GetSelectedLevelIndex() / 160f);

        while (hue < 0f)
        {
            hue += 1f;
        }

        Color targetColor = Color.HSVToRGB(hue, saturation, value);

        RenderSettings.skybox.SetColor("_GroundColor", targetColor);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
