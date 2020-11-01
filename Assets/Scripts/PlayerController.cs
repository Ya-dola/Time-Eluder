using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    public FloatingJoystick floatingJoystick;
    public Rigidbody playerRigBody;

    [Header("Player Speed")]
    public float playerSpeed;
    public int playerMinWalkingSpeed;
    [Range(0, 0.95f)]
    public float playerSlowDownSpeed;

    [Header("Camera")]
    public CameraController cameraController;
    public float minCamDistance;
    public float maxCamDistance;

    public float sideWallDistance;

    void Awake()
    {
        cameraController.cameraSpeed = playerMinWalkingSpeed;
    }

    void Update()
    {

    }

    public void FixedUpdate()
    {
        // Memory Improvements
        if (GameManager.singleton.GameEnded)
            return;

        // Make the player move when game starts 
        if (GameManager.singleton.GameStarted)
            playerRigBody.MovePosition(transform.position + Vector3.forward * playerMinWalkingSpeed * Time.fixedDeltaTime);

        Vector3 direction = Vector3.forward * floatingJoystick.Vertical + Vector3.right * floatingJoystick.Horizontal;
        playerRigBody.AddForce(direction * playerSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);

        // To reset the force applied on the player and reduce the force applied on it gradually 
        if (direction == Vector3.zero)
            playerRigBody.velocity = playerRigBody.velocity * playerSlowDownSpeed;

        // To stop applying force on the player after a threshold
        if (playerRigBody.velocity.magnitude < 0.1f)
            playerRigBody.velocity = Vector3.zero;
    }

    // Late Update used mainly for Camera Calculations and Calculations that need to occur after movement has occured
    // Occurs after physics is applied 
    private void LateUpdate()
    {
        Vector3 playerOldPos = transform.position;

        // To make the ball not fall from the floor through the sides
        if (transform.position.x < -sideWallDistance)
            playerOldPos.x = -sideWallDistance;

        else if (transform.position.x > sideWallDistance)
            playerOldPos.x = sideWallDistance;

        // To make the ball maintain a certain distance in front of the camera and never behind it
        float playerclosestPostion = Camera.main.transform.position.z + minCamDistance;

        if (transform.position.z < playerclosestPostion)
            playerOldPos.z = playerclosestPostion;

        // To make the ballnever extend a certain distance from the camera
        float playerfurthestPostion = Camera.main.transform.position.z + maxCamDistance;

        if (transform.position.z > playerfurthestPostion)
            playerOldPos.z = playerfurthestPostion;

        transform.position = playerOldPos;
    }
}
