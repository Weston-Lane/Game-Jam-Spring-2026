using KinematicCharacterController;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class EndCredits : MonoBehaviour
{
    #region Inspector Objects
    [Header("Object References")]
    [SerializeField] GameObject canvasRoot;
    //[Header("Configuration Variables")]

    //[Header("State")]
    #endregion
    
    Animator animator;
    const string IS_END = "IS_END";
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        
    }
    void OnAnimationComplete()
    {
        //Debug.Log($"[OBJ: " + transform + " SCRPT: EndCredits]: " +  "load main menu");
        SceneManager.LoadScene(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerController>(out var player))
        {
            canvasRoot.SetActive(true);
            animator.SetBool(IS_END , true);
        }
    }
}
