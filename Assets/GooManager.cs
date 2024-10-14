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


    private Vector3 MousePos;

    private void Start()
    {
        WaterGooCountCurrent = WaterGooCount;
        ElectricGooCountCurrent = ElectricGooCount;
        ConstructionGooCountCurrent = ConstructionGooCount;
        ElectricGoo.text = ElectricGooCountCurrent + "/" + ElectricGooCount;
        ConstructionGoo.text = ConstructionGooCountCurrent + "/" + ConstructionGooCount;
        WaterGoo.text = WaterGooCountCurrent + "/" + WaterGooCount;
    }

    private void Update()
    {
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