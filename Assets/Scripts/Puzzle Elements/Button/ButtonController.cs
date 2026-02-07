using UnityEngine;
using System.Collections.Generic;

public class ButtonController : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] private Animator animator;

    private List<Collider> colliders;

    public bool pressed = false;
    private int pressCount;

    private IToggleable toggleable;

    void Start()
    {
        toggleable = target.GetComponent<IToggleable>();
        colliders = new List<Collider>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Pole") return;

        colliders.Add(other);

        pressCount++;
        animator.Play("button_press");

        if (!pressed)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Button/Button Press", transform.position);
            toggleable.ToggleOn();
        } 
        
        pressed = true;
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Pole") return;

        colliders.Remove(other);

        pressCount--;
    }

    private void ToggleOn()
    {
        
    }

    private void ToggleOff()
    {
        
    }

    private void Update()
    {

        Collider colliderToRemove = null;
        foreach (var collider in colliders)
        {
            if (collider == null)
            {
                pressCount--;
                colliderToRemove = collider;
                Debug.Log("removed");
            }
        }

        colliders.Remove(colliderToRemove);
        colliderToRemove = null;

        if (pressCount <= 0)
        {
            if (pressed)
            {
                animator.Play("button_release");
                FMODUnity.RuntimeManager.PlayOneShot("event:/Button/Button Release", transform.position);
                toggleable.ToggleOff();
            } 

            pressed = false;
        }
    }
}
