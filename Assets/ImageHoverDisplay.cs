using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ImageHoverDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TMP_Text InfoText;
    [SerializeField] private LevelInfo LevelInfo;


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (LevelInfo != null)
        {
            string info = $"<b>Level:</b> {LevelInfo.levelName}\n" +
                          $"<b>Difficulty:</b> {LevelInfo.levelDifficulty}\n" +
                          $"<b>Required Goo:</b> {LevelInfo.requiredGooType}";

            InfoText.text = info;
            InfoText.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InfoText.gameObject.SetActive(false);
    }
}