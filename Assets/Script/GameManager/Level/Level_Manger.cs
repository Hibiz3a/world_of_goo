using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Level_Manger : MonoBehaviour
{
    [Header("Level Type and Goo")]
    [SerializeField] private LevelType CurrentLevelType;
    [SerializeField] private GooType CurrentGooType;

    [Header("GameObject")]
    [SerializeField] private GameObject StartGoo;
    [SerializeField] private GameObject EndGoo;
    [SerializeField] private GameObject PanelEndLevelWin;
    [SerializeField] private GameObject PanelEndLevelLoose;
    [SerializeField] private GameObject Canva;
    [SerializeField] private GameObject ObstaclePrefab;

    [Header("Vector3")]
    [SerializeField] private Vector3 FirstEncorStarGoo;
    [SerializeField] private Vector3 SecondEncorStarGoo;
    [SerializeField] private Vector3 FirstEncorEndGoo;
    [SerializeField] private Vector3 SecondEncorEndGoo;

    [Header("Int and Float")]
    [SerializeField] private int AmountOfStartGoo = 2;
    [SerializeField] private int baseObstacleCount = 3;
    [SerializeField] private float minDistanceBetweenObjects = 2f;
    
    private GooManager GooManager;
    private List<GameObject> Goos = new List<GameObject>();
    private List<GameObject> Obstacles = new List<GameObject>();
    private List<Vector3> usedPositions = new List<Vector3>();

    public GooType _CurrentGooType => CurrentGooType;
    public GooManager _GooManager => GooManager;

    public LevelType _CurrentLevelType => CurrentLevelType;

    private void Start()
    {
        GooManager = GooManager.instance;
        LoadGooForLevelAndType(CurrentLevelType, CurrentGooType);
    }

    public void SetLevelType(LevelType levelType)
    {
        CurrentLevelType = levelType;
        Debug.Log("Level Type Set to: " + levelType);
    }

    public void SetGooType(GooType gooType)
    {
        CurrentGooType = gooType;
        Debug.Log("Goo Type Set to: " + gooType);
    }

    #region SpawnGoo & Obstacles

    private void SpawnStartGoo()
    {
        List<Vector3> _posGoo = new List<Vector3>();
        for (int i = 0; i < AmountOfStartGoo; i++)
        {
            Vector3 posStartGoo;

            do
            {
                posStartGoo = new Vector3(Random.Range(FirstEncorStarGoo.x, SecondEncorStarGoo.x),
                    Random.Range(FirstEncorStarGoo.y, SecondEncorStarGoo.y), 0);
            } while (!IsPositionValid(posStartGoo, _posGoo));

            _posGoo.Add(posStartGoo);
            usedPositions.Add(posStartGoo);
        }

        for (int i = 0; i < _posGoo.Count; i++)
        {
            GameObject _startGoo =
                Instantiate(StartGoo, _posGoo[i], Quaternion.identity, GooManager.transform);
            Goos.Add(_startGoo);
        }
    }

    private void SpawnEndGoo()
    {
        Vector3 posEndGoo;

        do
        {
            posEndGoo = new Vector3(Random.Range(FirstEncorEndGoo.x, SecondEncorEndGoo.x),
                Random.Range(FirstEncorEndGoo.y, SecondEncorEndGoo.y), 0);
        } while (!IsPositionValid(posEndGoo, usedPositions));

        GameObject _endGoo = Instantiate(EndGoo, posEndGoo, Quaternion.identity, gameObject.transform);

        _endGoo.GetComponent<EndGooLevel>()._PanelEndLevel = PanelEndLevelWin;
        Goos.Add(_endGoo);
        usedPositions.Add(posEndGoo);
    }

    private void SpawnObstacles(LevelType levelType)
    {
        int obstacleCount = baseObstacleCount;

        switch (levelType)
        {
            case LevelType.Easy:
                obstacleCount = baseObstacleCount;
                break;
            case LevelType.Medium:
                obstacleCount = baseObstacleCount + 3;
                break;
            case LevelType.Hard:
                obstacleCount = baseObstacleCount + 6;
                break;
        }

        for (int i = 0; i < obstacleCount; i++)
        {
            Vector3 obstaclePosition;

            do
            {
                obstaclePosition = new Vector3(Random.Range(FirstEncorStarGoo.x, SecondEncorEndGoo.x),
                    Random.Range(FirstEncorStarGoo.y, SecondEncorEndGoo.y), 0);
            } while (!IsPositionValid(obstaclePosition, usedPositions));

            GameObject _obstacle = Instantiate(ObstaclePrefab, obstaclePosition, Quaternion.identity, this.transform);
            Obstacles.Add(_obstacle);
            usedPositions.Add(obstaclePosition);
        }
    }

    private bool IsPositionValid(Vector3 position, List<Vector3> positions)
    {
        foreach (Vector3 existingPosition in positions)
        {
            if (Vector3.Distance(position, existingPosition) < minDistanceBetweenObjects)
            {
                return false;
            }
        }

        return true;
    }

    private void AdjustEndPosition(LevelType levelType)
    {
        switch (levelType)
        {
            case LevelType.Easy:
                SecondEncorEndGoo += new Vector3(5, 0, 0);
                break;
            case LevelType.Medium:
                SecondEncorEndGoo += new Vector3(10, 0, 0);
                break;
            case LevelType.Hard:
                SecondEncorEndGoo += new Vector3(15, 0, 0);
                break;
        }
    }

    #endregion

    public void LoadGooForLevelAndType(LevelType levelType, GooType gooType)
    {
        SetLevelType(levelType);
        SetGooType(gooType);

        AdjustEndPosition(levelType);

        SpawnObstacles(levelType);

        SpawnGooOfType(CurrentGooType);
    }

    private void SpawnGooOfType(GooType gooType)
    {
        switch (gooType)
        {
            case GooType.Water:
                GooManager.WaterLevelLaunch();
                SpawnStartGoo();
                SpawnEndGoo();
                break;
            case GooType.Construction:
                GooManager.ConstructionLevelLaunch();
                SpawnStartGoo();
                SpawnEndGoo();
                break;
            case GooType.Electric:
                GooManager.ElectricGooLaunch();
                SpawnStartGoo();
                SpawnEndGoo();
                break;
            default:
                Debug.LogError("Unknown Goo Type!");
                break;
        }
    }

    #region EndLevel Functions

    public void EndLevelWin()
    {
        DestroyAllGooAndObstacles();
        PanelEndLevelWin.SetActive(true);
    }

    public void EndLevelLoose()
    {
        DestroyAllGooAndObstacles();
        PanelEndLevelLoose.SetActive(true);
    }

    private void DestroyAllGooAndObstacles()
    {
        for (int i = Goos.Count - 1; i >= 0; i--)
        {
            Destroy(Goos[i]);
            Goos.RemoveAt(i);
        }

        Canva.transform.GetChild(0).gameObject.SetActive(false);

        for (int i = GooManager._PlacedGoos.Count - 1; i >= 0; i--)
        {
            for (int j = GooManager._PlacedGoos[i].transform.childCount - 1; j >= 0; j--)
            {
                Destroy(GooManager._PlacedGoos[i].transform.GetChild(j).gameObject);
            }
            Destroy(GooManager._PlacedGoos[i]);
            GooManager._PlacedGoos.RemoveAt(i);
        }

        for (int i = Obstacles.Count - 1; i >= 0; i--)
        {
            Destroy(Obstacles[i]);
            Obstacles.RemoveAt(i);
        }
    }

    #endregion

    public void Restart()
    {
        LoadGooForLevelAndType(CurrentLevelType, CurrentGooType);
    }

    public void Exit()
    {
    }
}

public enum GooType
{
    Water,
    Construction,
    Electric,
    Start,
    End
}

public enum LevelType
{
    Easy,
    Medium,
    Hard
}
