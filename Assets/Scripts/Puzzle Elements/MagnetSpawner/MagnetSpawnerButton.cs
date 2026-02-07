using UnityEngine;

[RequireComponent(typeof(Transform))]
public class MagnetSpawnerButton : MonoBehaviour, IInteractable
{
    #region Inspector Objects
    //[Header("Object References")]
    [SerializeField] private Animator animator;

    //[Header("Configuration Variables")]

    //[Header("State")]

    #endregion

    MagnetSpawnerToggleable spawner;
    public void OnInteract()
    {
        spawner.SpawnMagnet();
        animator.Play("spawner_button_press");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Spawner Button/Spawner Press", transform.position);
        FMODUnity.RuntimeManager.PlayOneShot("event:/Chute/Chute Drop", spawner.transform.position);
    }
    void Start()
    {
        spawner = GetComponent<MagnetSpawnerToggleable>();   
    }
    
    void Update()
    {
        
    }
}
