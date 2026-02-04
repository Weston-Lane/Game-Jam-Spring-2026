using UnityEngine;

public class BaseMagnet : MonoBehaviour
{
    #region Inspector Objects
    [Header("Object References")]
    [SerializeField] Transform[] poles;

    [Header("Configuration Variables")]
    [SerializeField] float polePower;
    [SerializeField] float poleFlieldRadius;
    //[Header("State")]
    #endregion

    void Start()
    {
        foreach (var pole in poles)
        {
            pole.GetComponent<MagnetPole>().SetFieldRadius(poleFlieldRadius);
            pole.GetComponent<MagnetPole>().SetPower(polePower);
        }
    }
    private void OnValidate()
    {
        foreach (var pole in poles)
        {
            pole.GetComponent<MagnetPole>().SetFieldRadius(poleFlieldRadius);
            pole.GetComponent<MagnetPole>().SetPower(polePower);
        }
    }
}
