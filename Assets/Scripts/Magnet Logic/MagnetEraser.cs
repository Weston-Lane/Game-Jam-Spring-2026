using UnityEngine;

public class MagnetEraser : MonoBehaviour
{
    #region Inspector Objects
    [Header("Object References")]
    [SerializeField] Renderer renderer;

    //[Header("Configuration Variables")]

    [Header("State")]
    [SerializeField] Type type;
    #endregion
    enum Type
    {
        Destroy,
        PolarityNorth,
        PolaritySouth,
        UnCharge
    }
    Color destroyColor = Color.yellow, 
          polarityNorthColor = Color.red,
          PolaritySouthColor = Color.powderBlue, 
          UnChargeColor = Color.grey;
    const string COLOR = "_Color";
    void Start()
    {

        switch (type)
        {
            case Type.Destroy:
            {
                renderer.material.SetColor(COLOR, destroyColor);
            }
            break;
            case Type.PolarityNorth:
            {
                renderer.material.SetColor(COLOR, polarityNorthColor);
            }
            break;
            case Type.PolaritySouth:
            {
                renderer.material.SetColor(COLOR, PolaritySouthColor);
            }
            break;

            case Type.UnCharge:
            {
                renderer.material.SetColor(COLOR, UnChargeColor);
            }
            break;

            default:
            {

            }
            break;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<BaseMagnet>(out var magnet))
        {
            var poles = magnet.GetPoles();
            switch (type)
            {
                case Type.Destroy:
                {
                    Destroy(magnet.gameObject);
                }
                break;
                case Type.PolarityNorth:
                {
                    foreach(var pole in poles)
                    {
                        pole.ChangePolarity(BaseMagnet.Polarity.North);
                    }
                }
                break;
                case Type.PolaritySouth:
                {
                    foreach (var pole in poles)
                    {
                        pole.ChangePolarity(BaseMagnet.Polarity.South);
                    }
                }
                break;

                case Type.UnCharge:
                {
                    foreach (var pole in poles)
                    {
                        pole.ChangePolarity(BaseMagnet.Polarity.Uncharged);
                    }
                }
                break;

                default:
                {

                }
                break;
            }
        }
    }
}
