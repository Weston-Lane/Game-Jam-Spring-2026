using UnityEngine;

public class MainMenuMagTeleporter : MonoBehaviour
{
    #region Inspector Objects
    [Header("Object References")]
    [SerializeField] Transform teleportTo;
    //[Header("Configuration Variables")]

    //[Header("State")]
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<BaseMagnet>(out var mag))
            { other.transform.position = teleportTo.position; }
        
    }
}
