using System.Data.Common;
using UnityEngine;

public class MagnetPole : MonoBehaviour
{
    #region Inspector Objects
    //[Header("Object References")]

    [Header("Configuration Variables")]
    [SerializeField] bool isNorth;
    [SerializeField] float power;

    //[Header("State")]
    #endregion

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();    
    }

    private void OnTriggerStay(Collider collision)
    {
        Debug.Log(transform + " Has hit " + collision.transform);

        MagnetPole mp;
        if(collision.transform.TryGetComponent<MagnetPole>(out mp))
        {
            Vector3 dirVector = transform.position - collision.transform.position;
            dirVector = dirVector.normalized;
            float dist = Vector3.Distance(transform.position, collision.transform.position);
            Vector3 forceVector =
                dirVector / (dist * dist) * power * mp.power;

            if(mp.isNorth && isNorth ||
                !mp.isNorth && !isNorth)
                //if same pole repel
            {
                rb.AddForce(forceVector);
            }
            else
            //else attract
            {
                rb.AddForce(-forceVector);
            }
        }
        else
        {
            return;
        }
    }
}
