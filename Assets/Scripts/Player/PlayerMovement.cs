using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    CharacterController cc;

    [Header("Cursor Lock")]
    public CursorLockMode cursor;

    [Header("Arms Object")]
    public Transform arms;
    [Header("Flashlight Object")]
    public Transform flashlight;
    [Header("Cameras")]
    public Transform mainCamera;
    public Transform gunCamera;
    [Header("Movement Settings")]
    [Tooltip("How fast the player runs")]
    public float playerRunningSpeed = 10f;
    [Tooltip("How high the player jumps")]
    public float playerJumpingStrength = 5f;
    [Tooltip("How fast the player walks")]
    public float playerWalkingSpeed = 5f;

    [Header("Mouse Look Settings")]
    //How fast the player rotates when moving mouse
    [Tooltip("How fast the player rotates when moving mouse around")]
    public float normalLookSpeed = 2.0f;
    //How fast the player rotates when moving mouse
    [Tooltip("How fast the player rotates when aiming downsights")]
    public float aimLookSpeed = 2.0f;
    [Header("Mouse Look Clamp X Rotation")]

    float forwardMovement;
    float sidewaysMovement;
    float verticalMovement;
    Vector3 playerMovement;
    Quaternion playerRotation;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        Cursor.visible = false;
        Cursor.lockState = cursor;
    }


    float lookSpeed = 0f;
    [HideInInspector]
    public Vector2 rotation = new Vector2(0, 0);
    void Update()
    {
        // Mouse look
        // Different sensitivity when scoping (RMB)
        if (Input.GetKey(KeyCode.Mouse1))
            lookSpeed = aimLookSpeed;
        else
            lookSpeed = normalLookSpeed;

        rotation.y += (Input.GetAxis("Mouse X") * lookSpeed);
        rotation.x -= (Input.GetAxis("Mouse Y") * lookSpeed);

        //Rotate player on y axis based on mouse look speed
        transform.eulerAngles = new Vector2(0, rotation.y);
        //Clamp rotation on x axis to min and max values (prevents doing flips while standing)
        rotation.x = Mathf.Clamp(rotation.x, -90, 90);
        // Rotate cameras
        mainCamera.transform.eulerAngles = new Vector2(rotation.x, rotation.y);
        //Rotate arms on the x axis (y axis already rotated by rotating player)
        arms.transform.localRotation = Quaternion.Euler(rotation.x, 0, 0);
        // Rotate flashlight
        flashlight.transform.localRotation = Quaternion.Euler(rotation.x, 0, 0);

        //Running
        if (Input.GetKey(KeyCode.LeftShift) && cc.isGrounded)
        {
            forwardMovement = Input.GetAxis("Vertical") * playerRunningSpeed;
            sidewaysMovement = Input.GetAxis("Horizontal") * playerRunningSpeed;

            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
                DynamicCrosshair.changeSpread("run");
        }
        //Walking
        else if(cc.isGrounded)
        {
            forwardMovement = Input.GetAxis("Vertical") * playerWalkingSpeed;
            sidewaysMovement = Input.GetAxis("Horizontal") * playerWalkingSpeed;

            DynamicCrosshair.changeSpread("walk");
        }
        //Jumping
        verticalMovement += Physics.gravity.y * Time.deltaTime;
        if(Input.GetButton("Jump") && cc.isGrounded)
        {
            verticalMovement = playerJumpingStrength;
        }

        playerMovement.x = sidewaysMovement;
        playerMovement.y = verticalMovement;
        playerMovement.z = forwardMovement;

        if (cc.isGrounded)
        {
            playerRotation = cc.transform.rotation;
        }
        else
            DynamicCrosshair.changeSpread("jump");

        cc.Move(playerRotation * playerMovement * Time.deltaTime);
    }
}
