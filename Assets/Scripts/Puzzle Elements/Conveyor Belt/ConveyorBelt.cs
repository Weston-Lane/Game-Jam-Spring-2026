using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    [SerializeField] private float beltForce;
    [SerializeField] private Transform forwardTransform;


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
