using UnityEngine;
using static BaseMagnet;

public class MetallicWall : MonoBehaviour, IMetallic, IToggleable
{
    [SerializeField] private Renderer renderer;
    [SerializeField] private Polarity startPolarity;
    [SerializeField] private Transform wallNormal;
    [SerializeField] private int numInputs;

    private Polarity polarity;

    public int ToggleSources { get; set;}

    private float emissionBoost = 0;

    private void Start()
    {
        ChangePolarity(startPolarity);
    }

    public Polarity GetPolarity()
    {
        return polarity;
    }

    public Vector3 GetNormal()
    {
        return wallNormal.forward;
    }

    private void Update()
    {
        emissionBoost = MathHelpers.ExpDecay(emissionBoost, 0, 25, Time.deltaTime);
        renderer.material.SetFloat("_EmissionBoost", emissionBoost);
    }

    public void ToggleOn()
    {
        ToggleSources++; 
        if(ToggleSources < numInputs)
            { return; }

        if (startPolarity == Polarity.North) 
            ChangePolarity(Polarity.South);
        else
            ChangePolarity(Polarity.North);

        FMODUnity.RuntimeManager.PlayOneShot("event:/Magnets/Charge Magnet", transform.position);
    }

    public void ToggleOff()
    {
        ToggleSources--;
        if (ToggleSources > 0 || ToggleSources > numInputs) return;

        if (startPolarity == Polarity.North) 
            ChangePolarity(Polarity.North);
        else
            ChangePolarity(Polarity.South);

        FMODUnity.RuntimeManager.PlayOneShot("event:/Magnets/Charge Magnet");
    }

    public void ChangePolarity(Polarity polarity)
    {
        bool isNorth = polarity == Polarity.North;
        bool isUncharged = polarity == Polarity.Uncharged;
        renderer.material.SetFloat("_IsNorth", isNorth ? 1f : 0f);  
        renderer.material.SetFloat("_IsUncharged", isUncharged ? 1f : 0f);  
        this.polarity = polarity;
        emissionBoost = 1;
    }
}
