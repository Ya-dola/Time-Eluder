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
    public AudioSource playerRunningAudioSource { get; private set; }

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
    public float playerDashTimeDuration;
    public float playerSlowTimeDuration;

    [Header("Smoke Particles")]
    public float runningSmokeEmissionOverTime;
    public float dashSmokeEmissionOverDistance;

    [Header("Sounds")]
    public AudioClip playerDashSound;
    [Range(0, 1)]
    public float playerDashSoundVolume;
    public AudioClip playerSlowTimeSound;
    [Range(0, 1)]
    public float playerSlowTimeSoundVolume;

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
        playerRunningAudioSource = GetComponent<AudioSource>();
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
        playerRigBody.AddForce(direction * PlayerTimeFlow() * playerMoveSpeed * GameManager.singleton.playerSpeedFactor, ForceMode.VelocityChange);

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

        // Slow Time Logic for Player
        ActivatePlayerSlowTime();
    }

    void Update()
    {
        // To Start the Level and only starts it once if the game has already started
        if (!GameManager.singleton.GameStarted &&
            (joystick.Vertical != 0 || joystick.Horizontal != 0))
        {
            GameManager.singleton.StartGame();

            // Running Effect
            var runningSmokeEmission = playerSmokeParticles.emission;
            runningSmokeEmission.rateOverTime = runningSmokeEmissionOverTime;
        }


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
        if (joystick.Vertical > joystickAbilityZone &&
            GameManager.singleton.isDashCooldown == false &&
            GameManager.singleton.dashCount > 0 &&
            !GameManager.singleton.SlowTimeStatus)
        {
            GameManager.singleton.isDashCooldown = true;
            GameManager.singleton.darkDashImage.fillAmount = 1;

            StartCoroutine(PlayerDash());
        }

        // Dash Cooling Down
        if (GameManager.singleton.isDashCooldown)
        {
            GameManager.singleton.darkDashImage.fillAmount -= 1 / GameManager.singleton.dashCooldown * Time.fixedDeltaTime;

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

        // Dash Effects
        var dashSmokeEmission = playerSmokeParticles.emission;
        dashSmokeEmission.rateOverDistance = dashSmokeEmissionOverDistance;

        // Plays the sound between the Camera's position and the Player's position
        AudioSource.PlayClipAtPoint(playerDashSound, 0.9f * Camera.main.transform.position + 0.1f * transform.position, playerDashSoundVolume);

        GameManager.singleton.dashCount--;

        while (Time.time < startTime + playerDashTimeDuration)
        {
            Vector3 direction = Vector3.forward * verticalJoystickValue + Vector3.right * horizontalJoystickValue;
            playerRigBody.AddForce(direction * PlayerTimeFlow() * playerDashSpeed * GameManager.singleton.playerSpeedFactor, ForceMode.VelocityChange);

            yield return null;
        }

        // Dash Deactivated
        GameManager.singleton.DashEnded();
        dashSmokeEmission.rateOverDistance = 0;
    }

    private void ActivatePlayerSlowTime()
    {
        if (joystick.Vertical < -joystickAbilityZone &&
            GameManager.singleton.isSlowTimeCooldown == false &&
            GameManager.singleton.slowTimeCount > 0 &&
            !GameManager.singleton.DashStatus)
        {
            GameManager.singleton.isSlowTimeCooldown = true;
            GameManager.singleton.darkSlowTimeImage.fillAmount = 1;
            GameManager.singleton.activeSlowTimeImage.fillAmount = 1;

            StartCoroutine(SlowTime());
        }

        // Slow Time Cooling Down
        if (GameManager.singleton.isSlowTimeCooldown)
        {
            GameManager.singleton.darkSlowTimeImage.fillAmount -= 1 / GameManager.singleton.slowTimeCooldown * Time.fixedDeltaTime;

            if (GameManager.singleton.darkSlowTimeImage.fillAmount <= 0)
            {
                GameManager.singleton.darkSlowTimeImage.fillAmount = 0;
                GameManager.singleton.isSlowTimeCooldown = false;
            }
        }

        // Active Slow Time Indicator
        if (GameManager.singleton.SlowTimeStatus)
        {
            // TODO - Figure Out Equation on how 0.0244 is the value with Time Scale of 0.2
            GameManager.singleton.activeSlowTimeImage.fillAmount -= 0.0244f;

            if (GameManager.singleton.activeSlowTimeImage.fillAmount <= 0)
                GameManager.singleton.activeSlowTimeImage.fillAmount = 0;
        }
    }

    IEnumerator SlowTime()
    {
        float startTime = Time.time;

        // Slow Time Activated
        GameManager.singleton.SlowTimeStarted();

        // Plays the sound between the Camera's position and the Player's position
        AudioSource.PlayClipAtPoint(playerSlowTimeSound, 0.9f * Camera.main.transform.position + 0.1f * transform.position, playerSlowTimeSoundVolume);

        GameManager.singleton.slowTimeCount--;

        // playerSlowTimeDuration / GameManager.singleton.slowTimeFlow is the duration of Slow Time
        while (Time.time < startTime + playerSlowTimeDuration)
        {
            // To Pause Game while in Slow Time
            if (!GameManager.singleton.GamePaused)
            {
                GameManager.singleton.StartSlowTimeFlow();
                playerAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;

                yield return null;
            }
            else
            {
                playerAnimator.updateMode = AnimatorUpdateMode.Normal;
                yield return null;
            }
        }

        // Slow Time Deactivated
        DeactivateSlowTime();
    }

    private void DeactivateSlowTime()
    {
        playerAnimator.updateMode = AnimatorUpdateMode.Normal;

        GameManager.singleton.EndSlowTimeFlow();
        GameManager.singleton.SlowTimeEnded();
    }

    private float PlayerTimeFlow()
    {
        if (!GameManager.singleton.SlowTimeStatus)
            return Time.fixedDeltaTime;
        else
            return ((Time.fixedDeltaTime / Time.timeScale) * 2);
    }
}
