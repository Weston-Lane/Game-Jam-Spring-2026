using UnityEngine;

public class MenuManager : MonoBehaviour
{
    #region Inspector Objects
    [Header("Object References")]
    [SerializeField] UI_Elements ui_elements;
    //[Header("Configuration Variables")]

    [Header("State")]
    bool paused;
    

    [System.Serializable]
    class UI_Elements
    {
        public GameObject MenuUI;
    }
    #endregion

    void Start()
    {
        
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !paused)
        {
            HandlePause(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && paused)
        {
            HandlePause(false);
        }
    }
    void HandlePause(bool pause)
    {
        //if (pause)
        //{
        //    UI_Elements.MenuUI.gameObject.SetActive(true);
        //    Time.timeScale = 0;
        //    paused = true;
        //}
        //else
        //{
        //    UI_Elements.MenuUI.gameObject.SetActive(false);
        //    Time.timeScale = 1;
        //    paused = false;
        //}
    }
}
