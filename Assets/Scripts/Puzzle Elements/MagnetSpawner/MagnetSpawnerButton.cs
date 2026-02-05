using UnityEngine;

[RequireComponent(typeof(Transform))]
public class MagnetSpawnerButton : MonoBehaviour, IInteractable
{
    #region Inspector Objects
    //[Header("Object References")]

    //[Header("Configuration Variables")]

    //[Header("State")]

    #endregion

    MagnetSpawnerToggleable spawner;
    public void OnInteract()
    {
        spawner.SpawnMagnet();
    }
    void Start()
    {
        spawner = GetComponent<MagnetSpawnerToggleable>();   
    }
    
    void Update()
    {
        
    }
}
