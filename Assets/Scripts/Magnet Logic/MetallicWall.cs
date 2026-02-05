using UnityEngine;

public class MetallicWall : MonoBehaviour, IMetallic
{
    [SerializeField] private bool isNorth;
    [SerializeField] private bool hasCharge;

    public bool GetPolarity()
    {
        return isNorth;
    }
    public bool HasCharge()
    {
        return hasCharge;
    }
}
