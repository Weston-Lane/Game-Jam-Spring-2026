using UnityEngine;
using static BaseMagnet;

public class MetallicWall : MonoBehaviour, IMetallic
{
    [SerializeField] private Polarity polarity;
    [SerializeField] private Transform wallNormal;

    public Polarity GetPolarity()
    {
        return polarity;
    }

    public Vector3 GetNormal()
    {
        return wallNormal.forward;
    }
}
