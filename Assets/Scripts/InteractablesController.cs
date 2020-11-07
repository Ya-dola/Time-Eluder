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
    public float[] coinXPositions;
    public float[] coinZPosGapRange;

    [Header("Roadblock")]
    public GameObject roadblockPrefab;
    public Transform roadblockHolder;
    public Vector3 roadblockColliderHalfSize;
    public float[] roadblockXPositions;
    public float[] roadblockZPosGapRange;

    [Header("Damaged Roadblock")]
    public GameObject damagedRoadblockPrefab;
    public Transform damagedRoadblockHolder;
    public Vector3 damagedRoadblockColliderHalfSize;
    public float[] damagedRoadblockXPositions;
    public float[] damagedRoadblockZPosGapRange;

    [Header("Common Laser Turrets")]
    public float[] laserXPositions;
    public float[] laserZPosGapRange;

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

        // Generate Left or right Laser Turret Interactables
        GenerateLaserTurrets();

        // Generate Damaged Roadblock Interactables
        GenerateDamagedRoadblocks();

        // Generate Roadblock Interactables
        GenerateRoadblocks();

        // Generate Coin Interactables
        GenerateCoins();

    }

    private void DestroyExistingInteractables()
    {
        DestroyGameObjectChildren(leftLaserTurretHolder);
        DestroyGameObjectChildren(rightLaserTurretHolder);
        DestroyGameObjectChildren(damagedRoadblockHolder);
        DestroyGameObjectChildren(roadblockHolder);
        DestroyGameObjectChildren(coinHolder);
    }

    private void DestroyGameObjectChildren(Transform parentGameObject)
    {
        foreach (Transform item in parentGameObject)
        {
            Destroy(item.gameObject);
        }
    }

    private void GenerateLaserTurrets()
    {

    }

    private void GenerateDamagedRoadblocks()
    {

    }

    private void GenerateRoadblocks()
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

    private bool isPosColliderExist(Vector3 position, string objToGenerate)
    {
        switch (objToGenerate)
        {
            case "coin":
                if (Physics.CheckBox(position, coinColliderHalfSize))
                    return true;
                break;

            case "roadblock":
                if (Physics.CheckBox(position, roadblockColliderHalfSize, Quaternion.Euler(0f, 90f, 0)))
                    return true;
                break;

            case "damagedRoadblock":
                if (Physics.CheckBox(position, damagedRoadblockColliderHalfSize, Quaternion.Euler(0f, 90f, 0)))
                    return true;
                break;

            case "laser":
                break;

            default:
                break;
        }

        return false;
    }
}
