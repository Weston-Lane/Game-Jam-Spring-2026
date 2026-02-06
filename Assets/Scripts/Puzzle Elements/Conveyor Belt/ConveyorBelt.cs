using UnityEngine;

public class ConveyorBelt : MonoBehaviour, IMetallic
{
    [SerializeField] private float beltForce;
    [SerializeField] private Transform forwardTransform;
    [SerializeField] private Transform beltNormal;

    public Vector3 GetNormal()
    {
        return beltNormal.forward;
    }

    public BaseMagnet.Polarity GetPolarity()
    {
        return BaseMagnet.Polarity.Uncharged;
    }

    void OnTriggerStay(Collider other)
{
    if (!other.TryGetComponent<Rigidbody>(out var rb))
        return;

        Vector3 beltDir = forwardTransform.forward.normalized;

        // Remove existing velocity along belt
        Vector3 lateralVel = Vector3.Project(rb.linearVelocity, beltDir);

        // Apply conveyor speed directly
        Vector3 targetVel = beltDir * beltForce;

        // Preserve vertical & magnetic forces
        rb.linearVelocity = rb.linearVelocity - lateralVel + targetVel;
    }
}
