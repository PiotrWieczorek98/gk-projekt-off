using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTextureManager : MonoBehaviour
{

	public Camera cam;
	public Material cameraMat;

	// Use this for initialization
	void Start()
	{
		if (cam.targetTexture != null)
		{
			cam.targetTexture.Release();
		}
		cam.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
		cameraMat.mainTexture = cam.targetTexture;

	}

}