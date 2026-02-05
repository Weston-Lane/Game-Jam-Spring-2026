using UnityEngine;
using static BaseMagnet;

public class ChargePad : MonoBehaviour
{
    [SerializeField] private Renderer renderer;
    [SerializeField] private Polarity polarity;
    [SerializeField] private Light pointLight;
    [SerializeField] private Color northLight;
    [SerializeField] private Color southLight;

    void Start()
    {
        ChangePolarity(polarity);
    }

    private void ChangePolarity(Polarity polarity)
    {
        bool isNorth = polarity == Polarity.North;
        renderer.material.SetFloat("_IsNorth", isNorth ? 1f : 0f); 
        Debug.Log($"{polarity}");
        this.polarity = polarity;

        switch (polarity)
        {
            case Polarity.North:
                pointLight.color = northLight;
                break;
            case Polarity.South:
                pointLight.color = southLight;
                break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        MagnetPole pole = other.GetComponent<MagnetPole>();
        if (other.GetComponent<MagnetPole>() != null)
        {
            if (pole.polarity != polarity)
            {
                pole.ChangePolarity(polarity);
                FMODUnity.RuntimeManager.PlayOneShot("event:/Magnets/Charge Magnet");
            }
        }
    }
}
