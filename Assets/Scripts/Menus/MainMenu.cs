using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    #region Inspector Objects
    [Header("Object References")]
    [SerializeField] UIElements UI_Elements;
    [System.Serializable]
    class UIElements
    {
        public GameObject MenuUI;
    }
    //[Header("Configuration Variables")]

    //[Header("State")]
    #endregion

    AsyncOperation loadGameScene;
    const int MAIN_LEVEL_SCENE = 1;
    private void Start()
    {
        loadGameScene = SceneManager.LoadSceneAsync(MAIN_LEVEL_SCENE);
        loadGameScene.allowSceneActivation = false;
    }

    public void StartGame()
    {
        loadGameScene.allowSceneActivation = true;
    }
}
