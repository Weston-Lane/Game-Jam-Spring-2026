using System.Data.Common;

using UnityEngine;

public class MagnetPole : MonoBehaviour
{
    #region Inspector Objects
    [Header("Object References")]
    [SerializeField] Rigidbody PoleRb;
    [SerializeField] Transform[] neighborPoles;

    [Header("Configuration Variables")]
    [SerializeField] bool isNorth;
    [SerializeField] float power = 2f;
    [SerializeField] float fieldRadius = 0.5f;

    //[Header("State")]
    #endregion
    //comment
    SphereCollider sc;

    public void SetFieldRadius(float radius) => GetComponent<SphereCollider>().radius = radius;
    public void SetPower(float power) => this.power = power;
    void Start()
    {

        sc = GetComponent<SphereCollider>();

    }
    private void OnValidate()
    {
        sc = GetComponent<SphereCollider>();
        sc.radius = fieldRadius;
    }
    private void OnTriggerStay(Collider collision)
    {

        MagnetPole mp;
        if (collision.transform.TryGetComponent<MagnetPole>(out mp))
        {
            foreach (var neighbor in neighborPoles)
            //check if pole is a neighbor
            {
                if (collision.transform == neighbor)
                { return; }
            }

            Vector3 dirVector = transform.position - collision.transform.position;
            dirVector = dirVector.normalized;
            float dist = Vector3.Distance(transform.position, collision.transform.position);
            Vector3 forceVector =
                (dirVector / (dist * dist)) * power * mp.power;

            if (mp.isNorth && isNorth ||
                !mp.isNorth && !isNorth)
            //if same pole repel
            {
                PoleRb.AddForce(forceVector);
            }
            else
            //else attract
            {
                PoleRb.AddForce(-forceVector);
            }
        }
        else
        {
            return;
        }
    }



}
