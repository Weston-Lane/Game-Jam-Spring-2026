using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] private Animator animator;

    public bool pressed = false;
    private int pressCount;

    private IToggleable toggleable;

    void Start()
    {
        toggleable = target.GetComponent<IToggleable>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Pole") return;

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

        pressCount--;

        if (pressCount <= 0)
        {
            animator.Play("button_release");
            if (pressed)
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/Button/Button Release", transform.position);
                toggleable.ToggleOff();
            } 

            pressed = false;
        }
    }
}
