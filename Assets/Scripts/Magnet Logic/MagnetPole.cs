using UnityEngine;
using static BaseMagnet;

public class MagnetPole : MonoBehaviour
{
    #region Inspector Objects
    [Header("Object References")]
    [SerializeField] Rigidbody PoleRb;
    [SerializeField] Transform[] neighborPoles;

    [Header("Visuals")]
    [SerializeField] Renderer renderer;

    [Header("Configuration Variables")]
    [SerializeField] public Polarity polarity;
    [SerializeField] float power = 2f;
    [SerializeField] float fieldRadius = 0.5f;

    //[Header("State")]
    #endregion

    private float emissionBoost = 0;

    public void SetFieldRadius(float radius) => GetComponent<SphereCollider>().radius = radius;
    public void SetPower(float power) => this.power = power;
    void Start()
    {
        ChangePolarity(polarity);
    }

    public void Update()
    {
        emissionBoost = MathHelpers.ExpDecay(emissionBoost, 0, 25, Time.deltaTime);
        renderer.material.SetFloat("_EmissionBoost", emissionBoost);
    }

    private void OnTriggerStay(Collider collision)
    {

        MagnetPole mp;
        IMetallic metallic;

        //Is this another pole?
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

            if (mp.polarity == Polarity.North && polarity == Polarity.North || mp.polarity == Polarity.South && polarity == Polarity.South)
            //if same pole repel
            {
                PoleRb.AddForce(forceVector);
            }
            else if(mp.polarity == Polarity.North && polarity == Polarity.South ||
                    mp.polarity == Polarity.South && polarity == Polarity.North)
            //else attract
            {
                PoleRb.AddForce(-forceVector);
            }
            else
            //do nothing // Uncharged
            { }
        }

        //Or is this a metallic object
        else if (collision.transform.TryGetComponent<IMetallic>(out metallic))
        {
            Vector3 dirVector = metallic.GetNormal();
            dirVector = dirVector.normalized;
            float dist = Vector3.Distance(transform.position, collision.transform.position);
            Vector3 forceVector =
                (dirVector / (dist * dist)) * power;

            if (metallic.GetPolarity() == Polarity.Uncharged)
            {
                PoleRb.AddForce(-forceVector);
            }
            else if (metallic.GetPolarity() == Polarity.North && polarity == Polarity.North || 
                     metallic.GetPolarity() == Polarity.South && polarity == Polarity.South)
            {
                PoleRb.AddForce(forceVector);
            }
            else
            {
                PoleRb.AddForce(-forceVector);
            }
        }
        else
        {
            return;
        }
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
