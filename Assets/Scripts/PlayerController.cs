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
    public int playerMinWalkingSpeed = 5;

    public float sideWallDistance = 5f;
    public float minCamDistance = 4.5f;

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
    }

    // Late Update used mainly for Camera Calculations and Calculations that need to occur after movement has occured
    // Occurs after physics is applied 
    private void LateUpdate()
    {
        Vector3 ballOldPos = transform.position;

        // To make the ball not fall from the floor
        if (transform.position.x < -sideWallDistance)
            ballOldPos.x = -sideWallDistance;

        else if (transform.position.x > sideWallDistance)
            ballOldPos.x = sideWallDistance;

        // To make the ball always be in front of the camera and never behind it
        float ballLowestPostion = Camera.main.transform.position.z + minCamDistance;

        if (transform.position.z < ballLowestPostion)
            ballOldPos.z = ballLowestPostion;

        transform.position = ballOldPos;
    }
}
