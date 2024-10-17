using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class LoadScene : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int sceneIndex;  

    public void OnPointerClick(PointerEventData eventData)
    {
        LoadLevelScene(sceneIndex);  
    }

    public void LoadLevelScene(int _sceneLvl)
    {
        SceneManager.LoadScene(_sceneLvl);  
    }
}