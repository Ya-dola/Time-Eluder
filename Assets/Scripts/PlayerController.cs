using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    public FixedJoystick joystick;
    // public FloatingJoystick joystick;
    private Rigidbody playerRigBody;
    private Animator playerAnimator;
    private ParticleSystem playerSmokeParticles;

    [Header("Joystick")]
    [Range(0, 1)]
    public float joystickDeadZone;

    [Header("Player Speed")]
    public float playerMoveSpeed;
    [Range(0, 0.95f)]
    public float playerSlowDownSpeed;

    [Header("Abilities")]
    [Range(0, 1)]
    public float joystickAbilityZone;
    public float playerDashSpeed;
    public float playerDashTime;

    [Header("Smoke Particles")]
    public float dashSmokeEmissionOverDistance;

    private float verticalJoystickValue;
    private float horizontalJoystickValue;

    void Awake()
    {
        // Setting Default Values 
        verticalJoystickValue = 0;
        horizontalJoystickValue = 0;

        playerRigBody = GetComponent<Rigidbody>();
        playerAnimator = GetComponentInChildren<Animator>();
        playerSmokeParticles = GetComponentInChildren<ParticleSystem>();
    }

    public void FixedUpdate()
    {
        // Memory Improvements
        if (GameManager.singleton.GameEnded)
            return;

        // Make the player move at environment walking speed when game starts 
        if (GameManager.singleton.GameStarted)
            playerRigBody.MovePosition(transform.position + Vector3.forward * GameManager.singleton.environmentWalkingSpeed * Time.fixedDeltaTime);

        // General Movement of Player
        if (joystick.Vertical > joystickDeadZone)
            verticalJoystickValue = 1;
        else if (joystick.Vertical < -joystickDeadZone)
            verticalJoystickValue = -1;
        else
            verticalJoystickValue = 0;

        if (joystick.Horizontal > joystickDeadZone)
            horizontalJoystickValue = 1;
        else if (joystick.Horizontal < -joystickDeadZone)
            horizontalJoystickValue = -1;
        else
            horizontalJoystickValue = 0;

        // Vector3 direction = Vector3.forward * floatingJoystick.Vertical + Vector3.right * floatingJoystick.Horizontal;
        Vector3 direction = Vector3.forward * verticalJoystickValue + Vector3.right * horizontalJoystickValue;
        playerRigBody.AddForce(direction * Time.fixedDeltaTime * playerMoveSpeed * GameManager.singleton.playerSpeedFactor, ForceMode.VelocityChange);

        // To reset the force applied on the player and reducing it gradually 
        if (direction == Vector3.zero)
            playerRigBody.velocity = playerRigBody.velocity * playerSlowDownSpeed;

        // To stop applying force on the player after a threshold
        if (playerRigBody.velocity.magnitude < 0.1f)
            playerRigBody.velocity = Vector3.zero;

        // Debug.Log("Horizontal: " + floatingJoystick.Horizontal);
        // Debug.Log("Vertical: " + floatingJoystick.Vertical);

        // Dashing Logic for Player
        ActivatePlayerDash();
    }

    void Update()
    {
        playerAnimator.SetBool("GameStarted", GameManager.singleton.GameStarted);
        playerAnimator.SetBool("GameWon", GameManager.singleton.GameWon);
        playerAnimator.SetBool("GameLost", GameManager.singleton.GameLost);
        playerAnimator.SetBool("DashStatus", GameManager.singleton.DashStatus);
        playerAnimator.SetFloat("JoyPosX", joystick.Horizontal);
        playerAnimator.SetFloat("JoyPosY", joystick.Vertical);
    }

    // Late Update used mainly for Camera Calculations and Calculations that need to occur after movement has occured
    // Occurs after physics is applied 
    private void LateUpdate()
    {
        Vector3 playerOldPos = transform.position;

        // To make the ball not fall from the floor through the sides
        if (transform.position.x < -GameManager.singleton.sideWallDistance)
            playerOldPos.x = -GameManager.singleton.sideWallDistance;

        else if (transform.position.x > GameManager.singleton.sideWallDistance)
            playerOldPos.x = GameManager.singleton.sideWallDistance;

        // To make the ball maintain a certain distance in front of the camera and never behind it
        float playerclosestPostion = Camera.main.transform.position.z + GameManager.singleton.minCamDistance;

        if (transform.position.z < playerclosestPostion)
            playerOldPos.z = playerclosestPostion;

        // To make the ballnever extend a certain distance from the camera
        float playerfurthestPostion = Camera.main.transform.position.z + GameManager.singleton.maxCamDistance;

        if (transform.position.z > playerfurthestPostion)
            playerOldPos.z = playerfurthestPostion;

        transform.position = playerOldPos;
    }

    private void ActivatePlayerDash()
    {
        if (joystick.Vertical > joystickAbilityZone && GameManager.singleton.isDashCooldown == false && GameManager.singleton.playerDashCount > 0)
        {
            GameManager.singleton.isDashCooldown = true;
            GameManager.singleton.darkDashImage.fillAmount = 1;

            StartCoroutine(PlayerDash());
        }

        // Dash Cooling Down
        if (GameManager.singleton.isDashCooldown)
        {
            GameManager.singleton.darkDashImage.fillAmount -= 1 / GameManager.singleton.playerDashCooldown * Time.deltaTime;

            if (GameManager.singleton.darkDashImage.fillAmount <= 0)
            {
                GameManager.singleton.darkDashImage.fillAmount = 0;
                GameManager.singleton.isDashCooldown = false;
            }
        }
    }

    IEnumerator PlayerDash()
    {
        float startTime = Time.time;

        // Dash Activated
        GameManager.singleton.DashStarted();

        var dashSmokeEmission = playerSmokeParticles.emission;
        dashSmokeEmission.rateOverDistance = dashSmokeEmissionOverDistance;

        GameManager.singleton.playerDashCount--;

        while (Time.time < startTime + playerDashTime)
        {
            Vector3 direction = Vector3.forward * verticalJoystickValue + Vector3.right * horizontalJoystickValue;
            playerRigBody.AddForce(direction * Time.fixedDeltaTime * playerDashSpeed * GameManager.singleton.playerSpeedFactor, ForceMode.VelocityChange);

            yield return null;
        }

        // Dash Deactivated
        GameManager.singleton.DashEnded();
        dashSmokeEmission.rateOverDistance = 0;
    }
}
