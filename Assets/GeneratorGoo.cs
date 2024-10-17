using UnityEngine;
using UnityEngine.EventSystems;

public class GeneratorGoo : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject PanelGenerator;
    [SerializeField] private Level CurrenetLevel;

    public Level _CurrenetLevel => CurrenetLevel;

    private int ElectricGooMore;

    public int _ElectricGooMore => ElectricGooMore;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (CompareTag("Generator"))
        {
            PanelGenerator.SetActive(true);
        }
    }

    public void LevelUp()
    {
        switch (CurrenetLevel)
        {
            case Level.Level1:
                CurrenetLevel = Level.Level2;
                ElectricGooMore = 3;
                break;
            case Level.Level2:
                CurrenetLevel = Level.Level3;
                ElectricGooMore = 6;
                break;
            case Level.Level3:
                CurrenetLevel = Level.Level4;
                ElectricGooMore = 9;
                break;
            case Level.Level4:
                Debug.Log("you Can't Level Up !");
                break;
        }
    }
}