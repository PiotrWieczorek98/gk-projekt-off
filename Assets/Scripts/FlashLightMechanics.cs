using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightMechanics : MonoBehaviour
{
    public bool isFlashlightOn = false;
    public bool flashlightDelay = false;

    public GameObject flashlight;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            flashlightDelay = true;
            isFlashlightOn = !isFlashlightOn;
            flashlight.SetActive(isFlashlightOn);
            StartCoroutine(FlashlightDelay());
        }
    }

    IEnumerator FlashlightDelay()
    {
        yield return new WaitForSeconds(0.25f);
        flashlightDelay = false;
    }
}
