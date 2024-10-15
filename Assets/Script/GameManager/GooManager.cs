using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GooManager : MonoBehaviour
{
    private int WaterGooCount = 5;
    private int ElectricGooCount = 5;
    private int ConstructionGooCount = 5;
    private int WaterGooCountCurrent;
    private int ElectricGooCountCurrent;
    private int ConstructionGooCountCurrent;

    [SerializeField] private GameObject WaterGooGO;
    [SerializeField] private GameObject ElecticGooGO;
    [SerializeField] private GameObject ConstructionGooGO;

    [SerializeField] private TextMeshProUGUI ElectricGoo;
    [SerializeField] private TextMeshProUGUI WaterGoo;
    [SerializeField] private TextMeshProUGUI ConstructionGoo;


    public int _WaterGooCount
    {
        get { return WaterGooCount; }
        set { WaterGooCount = value; }
    }

    public int _ElectricGooCount
    {
        get { return ElectricGooCount; }
        set { ElectricGooCount = value; }
    }

    public int _ConstructionGooCount
    {
        get { return ConstructionGooCount; }
        set { ConstructionGooCount = value; }
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


    private Vector3 MousePos;

    private void Start()
    {
        ElectricGoo.text = ElectricGooCountCurrent + "/" + ElectricGooCount;
        ConstructionGoo.text = ConstructionGooCountCurrent + "/" + ConstructionGooCount;
        WaterGoo.text = WaterGooCountCurrent + "/" + WaterGooCount;
    }


    public void ConstructionLevelLaunch()
    {
        ConstructionGooCountCurrent = ConstructionGooCount;
        WaterGooCountCurrent = 0;
        ElectricGooCountCurrent = 0;
    }

    public void WaterLevelLaunch()
    {
        ConstructionGooCountCurrent = 0;
        WaterGooCountCurrent = WaterGooCount;
        ElectricGooCountCurrent = 0;
    }

    public void ElectricGooLaunch()
    {
        ConstructionGooCountCurrent = 0;
        WaterGooCountCurrent = 0;
        ElectricGooCountCurrent = ElectricGooCount;
    }


    public void WaterGooSpawn(InputAction.CallbackContext _context)
    {
        if (WaterGooCountCurrent > 0 && _context.started)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(WaterGooGO, mousePos, Quaternion.Euler(0, 0, 0), gameObject.transform);
            WaterGooCountCurrent--;
            WaterGoo.text = WaterGooCountCurrent + "/" + WaterGooCount;
        }
    }

    public void ElectricGooSpawn(InputAction.CallbackContext _context)
    {
        if (ElectricGooCountCurrent > 0 && _context.started)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(ElecticGooGO, mousePos, Quaternion.Euler(0, 0, 0), gameObject.transform);
            ElectricGooCountCurrent--;
            ElectricGoo.text = ElectricGooCountCurrent + "/" + ElectricGooCount;
        }
    }

    public void ConstructionGooSpawn(InputAction.CallbackContext _context)
    {
        if (ConstructionGooCountCurrent > 0 && _context.started)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(ConstructionGooGO, mousePos, Quaternion.Euler(0, 0, 0), gameObject.transform);
            ConstructionGooCountCurrent--;
            ConstructionGoo.text = ConstructionGooCountCurrent + "/" + ConstructionGooCount;
        }
    }
}