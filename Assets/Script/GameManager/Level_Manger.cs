using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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

    [SerializeField] private GameObject PanelEndLevel;
    [SerializeField] private GameObject Canva;

    private Dictionary<LevelType, GooType> LevelGooMapping;

    public GooManager _GooManager
    {
        get { return GooManager; }
    }

    public Dictionary<LevelType, GooType> _LevelGooMapping
    {
        get { return LevelGooMapping; }
    }

    public LevelType _CurrentLevelType
    {
        get { return CurrentLevelType; }
    }

    private void Start()
    {
        LevelGooMapping = new Dictionary<LevelType, GooType>
        {
            { LevelType.Easy, GooType.Construction },
            { LevelType.Medium, GooType.Water },
            { LevelType.Hard, GooType.Electric }
        };

        LoadGooForLevel(CurrentLevelType);
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
                Instantiate(StartGoo, _posGoo[i], Quaternion.Euler(0, 0, 0), GooManager.transform);
            }
        }
        else
        {
            SpawnStartGoo();
        }
    }

    public void EndLevel()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        Canva.transform.GetChild(0).gameObject.SetActive(false);
        for (int i = 0; i < GooManager.transform.childCount + 1; i++)
        {
            GooManager.transform.GetChild(i).gameObject.SetActive(false);
        }

        
    }

    private void SpawnEndGoo()
    {
        Vector3 posEndGoo = new Vector3(Random.Range(FirstEncorEndGoo.x, SecondEncorEndGoo.x),
            Random.Range(FirstEncorEndGoo.y, SecondEncorEndGoo.y), 0);

        GameObject _endGoo = Instantiate(EndGoo, posEndGoo, Quaternion.Euler(0, 0, 0), gameObject.transform);

        _endGoo.GetComponent<EndGooLevel>()._PanelEndLevel = PanelEndLevel;
    }

    private void LoadGooForLevel(LevelType levelType)
    {
        if (LevelGooMapping.TryGetValue(levelType, out GooType gooType))
        {
            CurrentGooType = gooType;
            SpawnGooOfType(CurrentGooType);
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
                SpawnEndGoo();
                Debug.Log("Spawning Water Goo...");
                break;
            case GooType.Construction:
                GooManager.ConstructionLevelLaunch();
                SpawnStartGoo();
                SpawnEndGoo();
                Debug.Log("Spawning Construction Goo...");
                break;
            case GooType.Electric:
                GooManager.ElectricGooLaunch();
                SpawnStartGoo();
                SpawnEndGoo();
                Debug.Log("Spawning Electric Goo...");
                break;
        }
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