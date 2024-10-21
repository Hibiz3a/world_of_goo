using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class LoadScene : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int sceneIndex;  

    public void OnPointerClick(PointerEventData eventData)
    {
        transform.parent.transform.parent.gameObject.SetActive(false);
        LoadLevelScene(sceneIndex);  
    }

    public void LoadLevelScene(int _sceneLvl)
    {
        SceneManager.LoadScene(_sceneLvl);  
    }
}