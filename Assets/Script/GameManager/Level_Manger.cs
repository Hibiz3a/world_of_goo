using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = System.Numerics.Vector2;

public class Level_Manger : MonoBehaviour
{
    [SerializeField] private LevelType currentLevelType;
    [SerializeField] private GooType currentGooType;
    [SerializeField] private GooManager GooManager;
    [SerializeField] private GameObject StartGoo;
    [SerializeField] private GameObject EndGoo;

    [SerializeField] private Vector3 FirstEncorStarGoo;
    [SerializeField] private Vector3 SecondEncorStarGoo;
    [SerializeField] private Vector3 FirstEncorEndGoo;
    [SerializeField] private Vector3 SecondEncorEndGoo;
    [SerializeField] private int AmountOfSartGoo = 2;


    private Dictionary<LevelType, GooType> levelGooMapping;

    private void Start()
    {
        levelGooMapping = new Dictionary<LevelType, GooType>
        {
            { LevelType.Easy, GooType.Construction },
            { LevelType.Medium, GooType.Water },
            { LevelType.Hard, GooType.Electric }
        };

        LoadGooForLevel(currentLevelType);
    }

    private void SpawnStartGoo()
    {
        for (int i = 0; i < AmountOfSartGoo; i++)
        {
            Vector3 posStargGoo = new Vector3(Random.Range(FirstEncorEndGoo.x, SecondEncorStarGoo.x),
                Random.Range(FirstEncorEndGoo.y, SecondEncorStarGoo.y), 0);

            Instantiate(StartGoo, posStargGoo, Quaternion.Euler(0, 0, 0), GooManager.transform);
        }
    }

    private void LoadGooForLevel(LevelType levelType)
    {
        if (levelGooMapping.TryGetValue(levelType, out GooType gooType))
        {
            currentGooType = gooType;
            SpawnGooOfType(currentGooType);
        }
        else
        {
            Debug.LogError("This level Type is not define ! ");
        }
    }

    private void SpawnGooOfType(GooType gooType)
    {
        switch (gooType)
        {
            case GooType.Water:
                GooManager.WaterLevelLaunch();
                SpawnStartGoo();
                Debug.Log("Spawning Water Goo...");
                break;
            case GooType.Construction:
                GooManager.ConstructionLevelLaunch();
                SpawnStartGoo();
                Debug.Log("Spawning Construction Goo...");
                break;
            case GooType.Electric:
                GooManager.ElectricGooLaunch();
                SpawnStartGoo();
                Debug.Log("Spawning Electric Goo...");
                break;
        }
    }
}

public enum GooType
{
    Water,
    Construction,
    Electric
}

public enum LevelType
{
    Easy,
    Medium,
    Hard
}