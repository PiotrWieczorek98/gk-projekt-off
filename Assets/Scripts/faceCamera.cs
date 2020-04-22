using UnityEngine;

public class faceCamera : MonoBehaviour
{
    public bool faceInY = false;
    Vector3 cameraDirection;
    void Update()
    {
        cameraDirection = Camera.main.transform.forward;
        if(faceInY == false)
            cameraDirection.y = 0;
        transform.rotation = Quaternion.LookRotation(cameraDirection);
    }
}
