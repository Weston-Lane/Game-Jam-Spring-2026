using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private string songPath;

    private void Start()
    {
        FMODUnity.RuntimeManager.PlayOneShot(songPath);
    }
}
