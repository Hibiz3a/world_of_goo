using System;
using TMPro;
using Unity.VisualScripting;
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
        GooLevelType = levelManager._LevelGooMapping[levelManager._CurrentLevelType];
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
                    levelManager._GooManager._ConstructionGooCount = CalculateGooEarned(
                        levelManager._GooManager._ConstructionGooCount,
                        levelManager._GooManager._CurrentConstructionGooCount, levelManager._CurrentLevelType);
                    PanelEndLevel.GetComponent<CanvasGroup>().alpha = 1;
                    PanelEndLevel.GetComponent<EndLevelPanel>()._FirstGain.text = "You have earned " +
                        levelManager._GooManager._ConstructionGooCount + " construction Goo.";
                        levelManager.EndLevel();
                    break;
                case GooType.Electric:
                    levelManager._GooManager._ElectricGooCount = CalculateGooEarned(
                        levelManager._GooManager._ElectricGooCount,
                        levelManager._GooManager._CurrentElectricGooCount, levelManager._CurrentLevelType);
                    PanelEndLevel.GetComponent<CanvasGroup>().alpha = 1;
                    PanelEndLevel.GetComponent<EndLevelPanel>()._SecondGain.text = "You have earned " +
                        levelManager._GooManager._ElectricGooCount + " electric Goo.";
                    levelManager.EndLevel();
                    break;
                case GooType.Water:
                    levelManager._GooManager._WaterGooCount = CalculateGooEarned(
                        levelManager._GooManager._WaterGooCount,
                        levelManager._GooManager._CurrentWaterGooCount, levelManager._CurrentLevelType);
                    PanelEndLevel.GetComponent<CanvasGroup>().alpha = 1;
                    PanelEndLevel.GetComponent<EndLevelPanel>()._ThirdGain.text = "You have earned " +
                        levelManager._GooManager._WaterGooCount + " water Goo.";
                    levelManager.EndLevel();
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
                    _earnedGoo = (_baseGooCount / 1) * 1;
                }

                break;
            case LevelType.Medium:
                if (_currentGooStock > 0)
                {
                    _earnedGoo = (_baseGooCount / _currentGooStock) * 2;
                }
                else
                {
                    _earnedGoo = (_baseGooCount / 1) * 2;
                }
                break;
            case LevelType.Hard:
                if (_currentGooStock > 0)
                {
                    _earnedGoo = (_baseGooCount / _currentGooStock) * 3;
                }
                else
                {
                    _earnedGoo = (_baseGooCount / 1) * 3;
                }
                break;
        }

        return _earnedGoo;
    }
}