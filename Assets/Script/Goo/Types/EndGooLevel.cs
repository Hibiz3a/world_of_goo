using UnityEngine;

public class EndGooLevel : MonoBehaviour
{
    private SpringJoint2D SpringJoint;

    private Level_Manger levelManager;
    [SerializeField] private GooType GooLevelType;
    private GameObject PanelEndLevel;

    public GameObject _PanelEndLevel
    {
        get { return PanelEndLevel; }
        set { PanelEndLevel = value; }
    }


    private void Start()
    {
        SpringJoint = gameObject.GetComponent<SpringJoint2D>();
        levelManager = gameObject.transform.parent.GetComponent<Level_Manger>();
        GooLevelType = levelManager._CurrentGooType;
    }

    private void Update()
    {
        if (SpringJoint.connectedBody != null)
        {
            LookIsAttach();
        }
    }


    private void LookIsAttach()
    {
        if (SpringJoint.connectedBody.gameObject.GetComponent<Attach_Goo>()._GooType == GooLevelType)
        {
            switch (GooLevelType)
            {
                case GooType.Construction:
                    int _earnedGooConstruction = CalculateGooEarned(
                        levelManager._GooManager._ConstructionGooCount,
                        levelManager._GooManager._CurrentConstructionGooCount, levelManager._CurrentLevelType);
                    levelManager._GooManager._ConstructionGooCount += _earnedGooConstruction;
                    PanelEndLevel.GetComponent<CanvasGroup>().alpha = 1;
                    PanelEndLevel.GetComponent<EndLevelPanel>()._FirstGain.gameObject.SetActive(true);
                    PanelEndLevel.GetComponent<EndLevelPanel>()._FirstGain.text = "You have earned " +
                        _earnedGooConstruction + " construction Goo.";
                    _earnedGooConstruction = 0;
                    levelManager.EndLevelWin();
                    break;
                case GooType.Electric:
                    int _earnedGooElectric = CalculateGooEarned(
                        levelManager._GooManager._ElectricGooCount,
                        levelManager._GooManager._CurrentElectricGooCount, levelManager._CurrentLevelType);
                    levelManager._GooManager._ElectricGooCount += _earnedGooElectric;
                    PanelEndLevel.GetComponent<CanvasGroup>().alpha = 1;
                    PanelEndLevel.GetComponent<EndLevelPanel>()._SecondGain.gameObject.SetActive(true);
                    PanelEndLevel.GetComponent<EndLevelPanel>()._SecondGain.text = "You have earned " +
                        _earnedGooElectric + " electric Goo.";
                    _earnedGooElectric = 0;
                    levelManager.EndLevelWin();
                    break;
                case GooType.Water:
                    int _earnedGooWater = CalculateGooEarned(
                        levelManager._GooManager._WaterGooCount,
                        levelManager._GooManager._CurrentWaterGooCount, levelManager._CurrentLevelType);
                    levelManager._GooManager._WaterGooCount += _earnedGooWater;
                    PanelEndLevel.GetComponent<CanvasGroup>().alpha = 1;
                    PanelEndLevel.GetComponent<EndLevelPanel>()._ThirdGain.gameObject.SetActive(true);
                    PanelEndLevel.GetComponent<EndLevelPanel>()._ThirdGain.text = "You have earned " +
                        _earnedGooWater + " water Goo.";
                    _earnedGooWater = 0;
                    levelManager.EndLevelWin();
                    break;
            }
        }
    }


    private int CalculateGooEarned(int _baseGooCount, int _currentGooStock, LevelType _levelType)
    {
        int _earnedGoo = 0;
        switch (_levelType)
        {
            case LevelType.Easy:
                if (_currentGooStock > 0)
                {
                    _earnedGoo = (_baseGooCount / _currentGooStock) * 1;
                }
                else
                {
                    _earnedGoo = 1;
                }

                break;
            case LevelType.Medium:
                if (_currentGooStock > 0)
                {
                    _earnedGoo = (_baseGooCount / _currentGooStock) * 2;
                }
                else
                {
                    _earnedGoo = 2;
                }
                break;
            case LevelType.Hard:
                if (_currentGooStock > 0)
                {
                    _earnedGoo = (_baseGooCount / _currentGooStock) * 3;
                }
                else
                {
                    _earnedGoo = 3;
                }
                break;
        }

        return _earnedGoo;
    }
}