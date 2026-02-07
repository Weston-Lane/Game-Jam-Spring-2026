using UnityEngine;

public class BaseMagnet : MonoBehaviour
{
    public enum Polarity{
        North,
        South,
        Uncharged,
    }

    #region Inspector Objects
    [Header("Object References")]
    [SerializeField] MagnetPole[] poles;

    [Header("Configuration Variables")]
    [SerializeField] float polePower = 1.5f;
    [SerializeField] float fieldRadius = 0.5f;
    //[Header("State")]
    #endregion

    public MagnetPole[] GetPoles() => poles;
    void Start()
    {
        foreach (var pole in poles)
        {
            pole.SetPower(polePower);
            pole.SetFieldRadius(fieldRadius);
        }
    }

    //allows us to change these fields easily in inspector
    private void OnValidate()
    {
        foreach (var pole in poles)
        {
            pole.SetPower(polePower);
            pole.SetFieldRadius(fieldRadius);
        }
    }
}
