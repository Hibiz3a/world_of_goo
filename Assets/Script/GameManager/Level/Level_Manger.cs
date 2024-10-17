using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Level_Manger : MonoBehaviour
{
    [SerializeField] private LevelType CurrentLevelType;
    [SerializeField] private GooType CurrentGooType;

    [SerializeField] private GooManager GooManager;
    [SerializeField] private GameObject StartGoo;
    [SerializeField] private GameObject EndGoo;

    [SerializeField] private Vector3 FirstEncorStarGoo;
    [SerializeField] private Vector3 SecondEncorStarGoo;
    [SerializeField] private Vector3 FirstEncorEndGoo;
    [SerializeField] private Vector3 SecondEncorEndGoo;

    [SerializeField] private int AmountOfSartGoo = 2;
    [SerializeField] private int baseObstacleCount = 3;

    [SerializeField] private GameObject PanelEndLevelWin;
    [SerializeField] private GameObject PanelEndLevelLoose;
    [SerializeField] private GameObject Canva;
    [SerializeField] private GameObject ObstaclePrefab;

    private List<GameObject> Goos = new List<GameObject>();
    private List<GameObject> Obstacles = new List<GameObject>();


    public GooType _CurrentGooType => CurrentGooType;
    public GooManager _GooManager => GooManager;

    public LevelType _CurrentLevelType => CurrentLevelType;

    private void Start()
    {
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

    private void SpawnStartGoo()
    {
        List<Vector3> _posGoo = new List<Vector3>();
        for (int i = 0; i < AmountOfSartGoo; i++)
        {
            Vector3 posStartGoo = new Vector3(Random.Range(FirstEncorStarGoo.x, SecondEncorStarGoo.x),
                Random.Range(FirstEncorStarGoo.y, SecondEncorStarGoo.y), 0);
            _posGoo.Add(posStartGoo);
        }

        if (Vector3.Distance(_posGoo[0], _posGoo[1]) >= 2)
        {
            for (int i = 0; i < _posGoo.Count; i++)
            {
                GameObject _startGoo =
                    Instantiate(StartGoo, _posGoo[i], Quaternion.Euler(0, 0, 0), GooManager.transform);
                Goos.Add(_startGoo);
            }
        }
        else
        {
            SpawnStartGoo();
        }
    }

    public void EndLevelWin()
    {
        for (int i = 0; i < Goos.Count; i++)
        {
            Destroy(Goos[i]);
            Goos.RemoveAt(i);
        }

        Canva.transform.GetChild(0).gameObject.SetActive(false);
        for (int i = 0; i < GooManager._PlacedGoos.Count; i++)
        {
            for (int j = 0; j < GooManager._PlacedGoos[i].transform.childCount; j++)
            {
                Destroy(GooManager._PlacedGoos[i].transform.GetChild(j).gameObject);
            }
            Destroy(GooManager._PlacedGoos[i]);
            GooManager._PlacedGoos.RemoveAt(i);
        }

        for (int i = 0; i < Obstacles.Count; i++)
        {
            Destroy(Obstacles[i]);
            Obstacles.RemoveAt(i);
        }
    }

    public void EndLevelLoose()
    {
        EndLevelWin();
        PanelEndLevelLoose.SetActive(true);
    }

    private void SpawnEndGoo()
    {
        Vector3 posEndGoo = new Vector3(Random.Range(FirstEncorEndGoo.x, SecondEncorEndGoo.x),
            Random.Range(FirstEncorEndGoo.y, SecondEncorEndGoo.y), 0);

        GameObject _endGoo = Instantiate(EndGoo, posEndGoo, Quaternion.Euler(0, 0, 0), gameObject.transform);

        _endGoo.GetComponent<EndGooLevel>()._PanelEndLevel = PanelEndLevelWin;
        Goos.Add(_endGoo);
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
            Vector3 obstaclePosition = new Vector3(Random.Range(FirstEncorStarGoo.x, SecondEncorEndGoo.x),
                Random.Range(FirstEncorStarGoo.y, SecondEncorEndGoo.y), 0);

            GameObject _obstacle = Instantiate(ObstaclePrefab, obstaclePosition, Quaternion.identity, this.transform);
            Obstacles.Add(_obstacle);
        }
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