using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseGoo : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject PanelLevel;


    private void Start()
    {
        PanelLevel = GooManager.instance._ChooseLevelPanel;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (CompareTag("Base"))
        {
            PanelLevel.SetActive(true);
            Debug.Log("Active");
        }
        else if (!CompareTag("Base") && !PanelLevel.activeSelf)
        {
            Debug.Log("Desactive");
            PanelLevel.SetActive(false);
        }
    }
}

public enum Level
{
    Level1,
    Level2,
    Level3,
    Level4
}