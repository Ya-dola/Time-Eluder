using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablesController : MonoBehaviour
{
    [Header("Interactables Logic")]
    public float interactableYPos;
    public int maxInteractablesGenerateAttempts;

    [Header("Coin")]
    public GameObject coinPrefab;
    public Transform coinHolder;
    public Vector3 coinColliderHalfSize;
    public float[] coinXPositions;
    public float[] coinZPosGapRange;
    public int coinMaxPerLine;
    public float coinYRotation;

    [Header("Roadblock")]
    public GameObject roadblockPrefab;
    public Transform roadblockHolder;
    public Vector3 roadblockColliderHalfSize;
    public float[] roadblockXPositions;
    public float[] roadblockZPosGapRange;
    public int roadblockMaxPerLine;
    public float roadblockYRotation;

    [Header("Damaged Roadblock")]
    public GameObject damagedRoadblockPrefab;
    public Transform damagedRoadblockHolder;
    public Vector3 damagedRoadblockColliderHalfSize;
    public float[] damagedRoadblockXPositions;
    public float[] damagedRoadblockZPosGapRange;
    public int damagedRoadblockMaxPerLine;
    public float damagedRoadblockYRotation;

    [Header("Common Laser Turrets")]
    public float[] laserXPositions;
    public float[] laserZPosGapRange;
    public int laserMaxPerLine;

    [Header("Left Laser Turret")]
    public GameObject leftLaserTurretPrefab;
    public Transform leftLaserTurretHolder;
    public float leftLaserTurretYRotation;

    [Header("Right Laser Turret")]
    public GameObject rightLaserTurretPrefab;
    public Transform rightLaserTurretHolder;
    public float rightLaserTurretYRotation;


    // Start is called before the first frame update
    void Start()
    {
        // Destroying Existing Interactables
        DestroyExistingInteractables();

        // Generate Left or right Laser Turret Interactables
        GenerateInteractables("laser", laserZPosGapRange[0], laserZPosGapRange[1], laserMaxPerLine,
                                leftLaserTurretHolder, leftLaserTurretPrefab, leftLaserTurretYRotation);

        // Generate Damaged Roadblock Interactables
        GenerateInteractables("roadblock", roadblockZPosGapRange[0], roadblockZPosGapRange[1], roadblockMaxPerLine,
                                roadblockHolder, roadblockPrefab, roadblockYRotation);
        // Generate Roadblock Interactables
        GenerateInteractables("damagedRoadblock", damagedRoadblockZPosGapRange[0], damagedRoadblockZPosGapRange[1], damagedRoadblockMaxPerLine,
                                damagedRoadblockHolder, damagedRoadblockPrefab, damagedRoadblockYRotation);
        // // Generate Coin Interactables
        GenerateInteractables("coin", coinZPosGapRange[0], coinZPosGapRange[1], coinMaxPerLine, coinHolder, coinPrefab, coinYRotation);
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

    private void GenerateInteractables(string interactableToGenerate, float interactableZPosGapLowRange, float interactableZPosGapHighRange,
    int interactableMaxPerLine, Transform interactableHolder, GameObject interactablePrefab, float interactableYRotation)
    {
        float currentZPos = GameManager.singleton.startLine.position.z;
        currentZPos += Mathf.Round(Random.Range(interactableZPosGapLowRange, interactableZPosGapHighRange));

        float interactableXPos;
        List<float> chosenXPos = new List<float>();
        int generateAttempts = 0;
        bool validPosition = false;

        do
        {
            interactableXPos = GetInteractableXPos(interactableToGenerate);

            while (chosenXPos.Count < interactableMaxPerLine && generateAttempts < maxInteractablesGenerateAttempts)
            {
                while (!validPosition && generateAttempts < maxInteractablesGenerateAttempts)
                {
                    generateAttempts++;
                    if (!chosenXPos.Contains(interactableXPos))
                        chosenXPos.Add(interactableXPos);

                    validPosition = !isPosColliderExist(new Vector3(interactableXPos, interactableYPos, currentZPos), interactableToGenerate);

                    if (!validPosition)
                        interactableXPos = GetInteractableXPos(interactableToGenerate);
                    // break;
                }

                if (validPosition)
                {
                    Instantiate(interactablePrefab, new Vector3(interactableXPos, interactableYPos, currentZPos),
                                Quaternion.Euler(0f, interactableYRotation, 0),
                                interactableHolder);
                }

                validPosition = false;
                // break;
            }

            generateAttempts = 0;
            chosenXPos.Clear();
            currentZPos += Mathf.Round(Random.Range(interactableZPosGapLowRange, interactableZPosGapHighRange));
        } while (currentZPos < GameManager.singleton.finishLine.position.z);
    }

    private float GetInteractableXPos(string interactableToGenerate)
    {
        switch (interactableToGenerate)
        {
            case "coin":
                return coinXPositions[Random.Range(0, coinXPositions.Length)];

            case "roadblock":
                return roadblockXPositions[Random.Range(0, roadblockXPositions.Length)];

            case "damagedRoadblock":
                return damagedRoadblockXPositions[Random.Range(0, damagedRoadblockXPositions.Length)];

            case "laser":
                return laserXPositions[Random.Range(0, laserXPositions.Length)];

            default:
                break;
        }
        return 0;
    }

    private bool isPosColliderExist(Vector3 position, string interactableToGenerate)
    {
        switch (interactableToGenerate)
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
