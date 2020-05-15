using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool opened = false;


    public void isOpened(bool opened)
    {
        this.opened = opened;
    }

    public bool isOpened() 
    {
        return this.opened;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
