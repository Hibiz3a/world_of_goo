using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void LoadLevelScene(int _sceneLvl)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (i == _sceneLvl)
            {
                SceneManager.LoadScene(i);
            }
        }
    }
}