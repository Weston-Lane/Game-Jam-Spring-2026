using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private Transform grabPoint;
    [SerializeField] private Transform headTransform;
    [SerializeField] private float range;

    public PhysicsObject heldObject;

    private void Update()
    {
        Ray ray = new Ray(headTransform.position, headTransform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, range))
        {
            IInteractable interactable = hit.transform.gameObject.GetComponent<IInteractable>();
            PhysicsObject physObject = hit.transform.gameObject.GetComponent<PhysicsObject>();

            if (Input.GetKeyDown(KeyCode.E))
            {
                //Check if physics object
                if (physObject != null && heldObject == null)
                {
                    PickUpPhysicsObject(physObject);

                    return;
                }

                //Check if normal interactable
                if (interactable != null)
                {
                    interactable.OnInteract();
                }
                
            }
        }

        //Drop Physics Object
        if (heldObject != null && Input.GetKeyDown(KeyCode.E))
        {
            heldObject.rb.useGravity = true;
            heldObject.rb.freezeRotation = false;

            heldObject = null;
        }
    }

    private void PickUpPhysicsObject(PhysicsObject physObject)
    {
        if (heldObject == null)
        {
            physObject.rb.useGravity = false;
            physObject.rb.angularVelocity = Vector3.zero;
            physObject.rb.freezeRotation = true;

            heldObject = physObject;

            return;
        }
    }

    private void FixedUpdate()
    {
        if (heldObject != null)
        {
            Vector3 toTarget = grabPoint.position - heldObject.transform.position;

            Vector3 force = toTarget * heldObject.strength - heldObject.rb.linearVelocity * heldObject.damping;

            heldObject.rb.AddForce(force, ForceMode.Force);
        }
    }
}
