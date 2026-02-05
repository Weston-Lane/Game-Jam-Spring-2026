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
        targetPosition = openPosition;   
    }

    public void ToggleOff()
    {
        ToggleSources--;
        if (ToggleSources > 0) return;

        targetPosition = closedPosition;
    }

    private void Update()
    {
        transform.position = MathHelpers.ExpDecay(transform.position, targetPosition, doorSpeed, Time.deltaTime);
    }
}
