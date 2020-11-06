using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablesController : MonoBehaviour
{
    [Header("Coin")]
    public GameObject coinPrefab;
    public Transform coinHolder;

    [Header("Roadblock")]
    public GameObject roadblockPrefab;
    public Transform roadblockHolder;
    
    [Header("Damaged Roadblock")]
    public GameObject damagedRoadblockPrefab;
    public Transform damagedRoadblockHolder;
    
    [Header("Left Laser Turret")]
    public GameObject leftLaserTurretPrefab;
    public Transform leftLaserTurretHolder;
    
    [Header("Right Laser Turret")]
    public GameObject rightLaserTurretPrefab;
    public Transform rightLaserTurretHolder;

    // Lists to keep track of interactable location within the game
    [HideInInspector]
    public List<GameObject> coinList;
    [HideInInspector]
    public List<GameObject> roadblockList;
    [HideInInspector]
    public List<GameObject> damagedRoadblockList;
    [HideInInspector]
    public List<GameObject> leftLaserTurretList;
    [HideInInspector]
    public List<GameObject> rightLaserTurretList;


    // Start is called before the first frame update
    void Start()
    {
        // Initialising the lists
        coinList = new List<GameObject>();
        roadblockList = new List<GameObject>();
        damagedRoadblockList = new List<GameObject>();
        leftLaserTurretList = new List<GameObject>();
        rightLaserTurretList = new List<GameObject>();

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
