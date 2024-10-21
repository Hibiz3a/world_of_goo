using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GooManager : MonoBehaviour
{
    [SerializeField] private int WaterGooCount = 5;
    [SerializeField] private int ElectricGooCount = 5;
    [SerializeField] private int ConstructionGooCount = 5;
    [SerializeField] private int WaterGooCountCurrent;
    [SerializeField] private int ElectricGooCountCurrent;
    [SerializeField] private int ConstructionGooCountCurrent;

    [SerializeField] private GameObject WaterGooGO;
    [SerializeField] private GameObject ElecticGooGO;
    [SerializeField] private GameObject ConstructionGooGO;
    [SerializeField] private GameObject PanelEndLevelWin;
    [SerializeField] private GameObject PanelEndLevelLoose;
    [SerializeField] private GameObject ChooseLevelPanel;

    public GameObject _ChooseLevelPanel
    {
        get => ChooseLevelPanel;
        set => ChooseLevelPanel = value;
    }

    [SerializeField] private GameObject GooCountPanel;
    [SerializeField] private GameObject Tips;


    [SerializeField] private GameObject Canva;

    [SerializeField] private TextMeshProUGUI ElectricGoo;
    [SerializeField] private TextMeshProUGUI WaterGoo;
    [SerializeField] private TextMeshProUGUI ConstructionGoo;


    private List<GameObject> PlacedGoos = new List<GameObject>();

    #region Getter & Setter

    public GameObject _Tips
    {
        get => Tips;
        set => Tips = value;
    }

    public GameObject _PanelEndLevelWin
    {
        get => PanelEndLevelWin;
        set => PanelEndLevelWin = value;
    }

    public GameObject _PanelEndLevelLoose
    {
        get => PanelEndLevelLoose;
        set => PanelEndLevelLoose = value;
    }

    public GameObject _Canva
    {
        get => Canva;
        set => Canva = value;
    }


    public List<GameObject> _PlacedGoos
    {
        get => PlacedGoos;
        set => PlacedGoos = value;
    }

    public int _WaterGooCount
    {
        get { return WaterGooCount; }
        set { WaterGooCount = value; }
    }

    public int _ElectricGooCount
    {
        get => ElectricGooCount;
        set => ElectricGooCount = value;
    }

    public int _ConstructionGooCount
    {
        get => ConstructionGooCount;
        set => ConstructionGooCount = value;
    }

    public int _CurrentWaterGooCount
    {
        get { return WaterGooCountCurrent; }
    }

    public int _CurrentElectricGooCount
    {
        get { return ElectricGooCountCurrent; }
    }

    public int _CurrentConstructionGooCount
    {
        get { return ConstructionGooCountCurrent; }
    }

    #endregion

    private Vector3 MousePos;

    public static GooManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);


        ElectricGooCountCurrent = ElectricGooCount;
        WaterGooCountCurrent = WaterGooCount;
        ConstructionGooCountCurrent = ConstructionGooCount;
        ElectricGoo.text = ElectricGooCountCurrent + "/" + ElectricGooCount;
        ConstructionGoo.text = ConstructionGooCountCurrent + "/" + ConstructionGooCount;
        WaterGoo.text = WaterGooCountCurrent + "/" + WaterGooCount;
    }


    public void ConstructionLevelLaunch()
    {
        Tips.SetActive(true);
        Tips.transform.GetChild(0).gameObject.SetActive(true);
        ConstructionGooCountCurrent = ConstructionGooCount;
        WaterGooCountCurrent = 0;
        ElectricGooCountCurrent = 0;
    }

    public void WaterLevelLaunch()
    {
        Tips.SetActive(true);
        Tips.transform.GetChild(1).gameObject.SetActive(true);
        ConstructionGooCountCurrent = 0;
        WaterGooCountCurrent = WaterGooCount;
        ElectricGooCountCurrent = 0;
    }

    public void ElectricGooLaunch()
    {
        Tips.SetActive(true);
        Tips.transform.GetChild(2).gameObject.SetActive(true);
        ConstructionGooCountCurrent = 0;
        WaterGooCountCurrent = 0;
        ElectricGooCountCurrent = ElectricGooCount;
    }


    public void WaterGooSpawn(InputAction.CallbackContext _context)
    {
        if (WaterGooCountCurrent > 0 && _context.started)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject _waterGoo = Instantiate(WaterGooGO, mousePos, Quaternion.Euler(0, 0, 0), gameObject.transform);
            PlacedGoos.Add(_waterGoo);
            WaterGooCountCurrent--;
            WaterGoo.text = WaterGooCountCurrent + "/" + WaterGooCount;
        }
    }

    public void ElectricGooSpawn(InputAction.CallbackContext _context)
    {
        if (ElectricGooCountCurrent > 0 && _context.started)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject _electricGoo =
                Instantiate(ElecticGooGO, mousePos, Quaternion.Euler(0, 0, 0), gameObject.transform);
            PlacedGoos.Add(_electricGoo);
            ElectricGooCountCurrent--;
            ElectricGoo.text = ElectricGooCountCurrent + "/" + ElectricGooCount;
        }
    }

    public void ConstructGooSpawn(InputAction.CallbackContext _context)
    {
        if (ConstructionGooCountCurrent > 0 && _context.started)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject _constructionGoo =
                Instantiate(ConstructionGooGO, mousePos, Quaternion.Euler(0, 0, 0), gameObject.transform);
            PlacedGoos.Add(_constructionGoo);
            ConstructionGooCountCurrent--;
            ConstructionGoo.text = ConstructionGooCountCurrent + "/" + ConstructionGooCount;
        }
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(1);
        PanelEndLevelLoose.SetActive(false);
        PanelEndLevelWin.SetActive(false);
        GooCountPanel.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
    
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GooCountPanel.SetActive(true);
        PanelEndLevelWin.SetActive(false);
        PanelEndLevelLoose.SetActive(false);
    }
}