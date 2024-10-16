using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;


public class BaseGoo : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject PanelLevel;

    private void Start()
    {
        Debug.Log("click");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("click");
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