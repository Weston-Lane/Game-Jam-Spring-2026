using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private Transform grabPoint;
    [SerializeField] private Transform headTransform;
    [SerializeField] private float range;
    [SerializeField] private float heldItemRotationSpeed = 5f;

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
        if (heldObject != null && Input.GetKey(KeyCode.R))
        //if R is held then lock camera and rotate obj in hand
        {
            headTransform.GetComponent<MouseLook>().SetCameraLock(true);
            Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            heldObject.transform.Rotate(headTransform.up, -mouseDelta.x * heldItemRotationSpeed, Space.World);
            heldObject.transform.Rotate(headTransform.right, mouseDelta.y * heldItemRotationSpeed, Space.World);

        }
        else
            { headTransform.GetComponent<MouseLook>().SetCameraLock(false); }
        
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

