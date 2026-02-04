
// using System.Numerics;
using UnityEngine;

public class PhysicsObject : MonoBehaviour, IInteractable
{
    [Header("Grab Settings")]
    public float strength;
    public float damping;

    public Rigidbody rb;

    private Transform grabPoint;
    private bool beingHeld;

    public void OnInteract()
    {
        // this.grabPoint = grabPoint.transform;

        // if (!beingHeld)
        // {
        //     rb.useGravity = false;
        //     rb.angularVelocity = Vector3.zero;
        //     rb.freezeRotation = true;

        //     beingHeld = true;
        //     return;
        // } 

        // if (beingHeld)
        // {
        //     rb.useGravity = true;
        //     rb.freezeRotation = false;

        //     beingHeld = false;
        //     return;
        // }    
    }

    private void FixedUpdate()
    {
        if (beingHeld && grabPoint != null)
        {
            Vector3 toTarget = grabPoint.position - this.transform.position;

            Vector3 force = toTarget * strength - rb.linearVelocity * damping;

            rb.AddForce(force, ForceMode.Force);
        }
    }
}
