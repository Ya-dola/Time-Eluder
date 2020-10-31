using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // TODO - Add Headers and group fields
    public static GameManager singleton;
    public bool GameStarted { get; private set; }
    public bool GameEnded { get; private set; }

    [SerializeField] private Transform startTransform;
    [SerializeField] private Transform finishTransform;
    [SerializeField] private PlayerController playerController;

    [SerializeField] private float slowMotionFactor = 0.1f;
    [SerializeField] private float deltaTime = 0.02f;
    [SerializeField] private int transitionTime = 3;

    public float EntireDistance { get; private set; }
    public float RemainingDistance { get; private set; }

    // public GameObject youWonText;
    // public GameObject youDiedText;

    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else if (singleton != this)
            Destroy(gameObject);

        // Specifies Default time flow
        Time.timeScale = 1f;
        Time.fixedDeltaTime = deltaTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        EntireDistance = finishTransform.position.z - startTransform.position.z;

        // Signals the Game has started and only runs it once if the game has already started
        if (!GameManager.singleton.GameStarted)
            GameManager.singleton.StartGame();

        // youWonText.SetActive(false);
        // youDiedText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        RemainingDistance = Vector3.Distance(playerController.transform.position, finishTransform.position);

        // If Player is behind start line then the distance is the distance between the start and finish lines 
        if (RemainingDistance > EntireDistance)
            RemainingDistance = EntireDistance;

        // To avoid negative distance being shown to the player if the Player has passed the finish line
        if (playerController.transform.position.z > finishTransform.transform.position.z)
            RemainingDistance = 0;
    }

    public void StartGame()
    {
        GameStarted = true;

        // TODO - Change to have a Button in UI to start the game
        // Base the buttons off of what was used in Grid Generator
        // if (Input.GetKeyDown("w"))
        // {
        //     // Signals the Game has started and only runs it once if the game has already started
        //     if (!GameManager.singleton.GameStarted)
        //         GameManager.singleton.StartGame();
        //     Debug.Log("Game Started !!!");
        // }

    }

    // TODO - Change how this works to be easier to reuse
    public void EndGame(bool gameWon)
    {
        GameEnded = true;

        if (!gameWon)
        {
            AddSlowMotionEffect("RestartGame", transitionTime);

            // Debug.Log("Death Obstracle hit !!!");
            // youDiedText.SetActive(true);
        }
        else
        {
            Invoke("RestartGame", transitionTime);

            // Debug.Log("Finish Line Reached !!!");
            // youWonText.SetActive(true);
        }
    }

    public void RestartGame()
    {
        // Scene Numbers are according to those shown in Build
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    private void AddSlowMotionEffect(string method, int slowMotionTime)
    {
        // Calls Method after Specified time
        Invoke(method, slowMotionTime * slowMotionFactor);

        // Creates Slow motion time flow effect 
        Time.timeScale = slowMotionFactor;
        Time.fixedDeltaTime = deltaTime * Time.timeScale;
    }
}
