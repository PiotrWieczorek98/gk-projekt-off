using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowDoor : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    public void Start()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
    }

    public void TurnOnDoor()
    {
        meshRenderer.enabled = true;
    }
}
