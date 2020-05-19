using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightMechanics : MonoBehaviour
{
    public bool enableFlashlight = false;
    private bool toggleFlashlight = false;
    private Light flashlight;
    private KeyCode flashlightKey;

    void Start()
    {
        if (enableFlashlight)
        {
            flashlight = GetComponent<Light>();

            if (PlayerPrefs.HasKey("Flashlight"))
                flashlightKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Flashlight"));
            else
                flashlightKey = KeyCode.F;
        }
        else
            this.gameObject.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(flashlightKey))
        {
            toggleFlashlight = !toggleFlashlight;
            flashlight.enabled = toggleFlashlight;
        }
    }


}
