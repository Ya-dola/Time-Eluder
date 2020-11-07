using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablesController : MonoBehaviour
{
    [Header("Interactables Y Position")]
    public float yPos;

    [Header("Coin")]
    public GameObject coinPrefab;
    public Transform coinHolder;
    public Vector3 coinColliderHalfSize;

    [Header("Roadblock")]
    public GameObject roadblockPrefab;
    public Transform roadblockHolder;
    public Vector3 roadblockColliderHalfSize;

    [Header("Damaged Roadblock")]
    public GameObject damagedRoadblockPrefab;
    public Transform damagedRoadblockHolder;
    public Vector3 damagedRoadblockColliderHalfSize;

    [Header("Left Laser Turret")]
    public GameObject leftLaserTurretPrefab;
    public Transform leftLaserTurretHolder;

    [Header("Right Laser Turret")]
    public GameObject rightLaserTurretPrefab;
    public Transform rightLaserTurretHolder;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // if (Physics.CheckBox(new Vector3(-2f, yPos, 22f), coinColliderHalfSize))
        //     Debug.Log("Coin Colliding");
        // if (Physics.CheckBox(new Vector3(-4f, yPos, 32f), roadblockColliderHalfSize, Quaternion.Euler(0f, 90f, 0)))
        //     Debug.Log("Roadblock Colliding");
        // if (Physics.CheckBox(new Vector3(-3f, yPos, 26f), damagedRoadblockColliderHalfSize, Quaternion.Euler(0f, 90f, 0)))
        //     Debug.Log("Damaged Roadblock Colliding");
    }

    public void GenerateInteractables()
    {
        // Destroying Existing Interactables if any
        DestroyExistingInteractables();

        // Generate Coin Interactables
        GenerateCoins();

        // Generate Roadblock Interactables
        GenerateRoadblocks();

        // Generate Damaged Roadblock Interactables
        GenerateDamagedRoadblocks();

        // Generate Left Laser Turret Interactables
        GenerateLeftLaserTurret();

        // Generate Right Laser Turret Interactables
        GenerateRightLaserTurret();

    }

    private void DestroyExistingInteractables()
    {

    }



    private void GenerateCoins()
    {
        // int positionX;
        // int positionZ;

        // GameObject gameObject;

        // for (int i = 0; i < size; i++)
        // {
        //     while (true)
        //     {
        //         positionX = UnityEngine.Random.Range(0, GridWidth);
        //         positionZ = UnityEngine.Random.Range(0, GridHeight);

        //         if (!IsCellOccupied(new Vector3(positionX, positionY, positionZ)))
        //         {
        //             gameObject = Instantiate(prefab, new Vector3(positionX, positionY, positionZ), Quaternion.identity, parent);
        //             createdGameObjectsList.Add(gameObject);
        //             break;
        //         }
        //         else
        //         {
        //             Debug.Log("Cell Occupied at: (" + positionX + ", " + positionY + ", " + positionZ + ") - Recreating: " + prefab.name);
        //         }
        //     }
        // }
    }
    private void GenerateRoadblocks()
    {

    }
    private void GenerateDamagedRoadblocks()
    {

    }
    private void GenerateLeftLaserTurret()
    {

    }

    private void GenerateRightLaserTurret()
    {

    }

    private void isPosColliderExist()
    {

    }
}
