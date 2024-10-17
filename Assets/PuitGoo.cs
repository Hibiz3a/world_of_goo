using UnityEngine;
using UnityEngine.EventSystems;

public class PuitGoo : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject PanelWell;


    public void OnPointerClick(PointerEventData eventData)
    {
        if (CompareTag("Well"))
        {
            PanelWell.SetActive(true);
        }
    }
}