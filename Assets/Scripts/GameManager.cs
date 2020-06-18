using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject completeLevelUI;

    public void CompleteLevel()
    {
        completeLevelUI.SetActive(true);
    }


    // Update is called once per frame
    void Start()
    {
        completeLevelUI.SetActive(false);
    }
}
