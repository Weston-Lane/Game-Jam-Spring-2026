using UnityEngine;

public class DoorController : MonoBehaviour, IToggleable
{
    public  enum StartPosition
    {
        Open,
        Closed
    }

    public StartPosition startPosition = StartPosition.Closed;
    [SerializeField] private float doorSpeed;
    [SerializeField] private float doorOpenHeight;
    [SerializeField] private AnimationCurve curve;

    [Header("Number of inputs to open door")]
    [SerializeField] private int numInputs;

    private float doorProgress;
    private float doorTarget;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private Vector3 targetPosition;

    //Multiple buttons can toggle the same object, this keeps track of how many
    public int ToggleSources {get; set;}

    void Awake()
    {
        switch (startPosition)
        {
            case StartPosition.Closed:
                closedPosition = transform.position;
                openPosition = transform.position + new Vector3(0, doorOpenHeight, 0);
                break;
            case StartPosition.Open:
                openPosition = transform.position;
                closedPosition = transform.position + new Vector3(0, doorOpenHeight, 0);
                break;
        }
        
        targetPosition = closedPosition;
    }


    public void ToggleOn()
    {
        ToggleSources++; 
        if (ToggleSources < numInputs) return;
        
        targetPosition = openPosition;   
        doorTarget = 1;

        FMODUnity.RuntimeManager.PlayOneShot("event:/Door/Door Open", transform.position);
    }

    public void ToggleOff()
    {
        ToggleSources--;
        if (ToggleSources > numInputs) return;

        targetPosition = closedPosition;
        doorTarget = 0;
        
        FMODUnity.RuntimeManager.PlayOneShot("event:/Door/Door Close", transform.position);
    }

    private void Update()
    {
        doorProgress = MathHelpers.ExpDecay(doorProgress, doorTarget, doorSpeed, Time.deltaTime);
        transform.position = Vector3.Lerp(closedPosition, openPosition, curve.Evaluate(doorProgress));
    }
}
