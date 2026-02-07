using UnityEngine;
using FMOD.Studio;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private string songPath;

    private EventInstance musicInstance;

    private void Awake()
    {
        musicInstance = FMODUnity.RuntimeManager.CreateInstance(songPath);
    }

    private void Start()
    {
        musicInstance.start();
    }

    private void OnDestroy()
    {
        musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        musicInstance.release();
    }
}