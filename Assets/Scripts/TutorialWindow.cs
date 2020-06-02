using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialWindow : MonoBehaviour
{
    public Image tutorialImg;

    bool wasE = false;
    bool wasWSAD = false;
    bool wasSpace = false;
    bool wasWheel = false;
    bool wasShoot = false;
    // Start is called before the first frame update
    void Start()
    {
        tutorialImg.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            wasE = true;
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            wasWSAD = true;
        if (Input.GetKeyDown(KeyCode.Space))
            wasSpace = true;
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3) || Input.GetAxis("Mouse ScrollWheel") > 0 || Input.GetAxis("Mouse ScrollWheel") < 0)
            wasWheel = true;
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            wasShoot = true;

        if (wasE && wasWSAD && wasSpace && wasWheel && wasShoot)
            tutorialImg.enabled = false;
    }
}
