using UnityEngine;
using UnityEngine.Events;

public class InteractEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent interactEvent;
    [SerializeField] private bool canClickInteract = true;
    [SerializeField] private int interactCount = 1;
    [SerializeField] private float delayToCall = 0;

    public void CallInteract()
    {
        if (interactCount > 0)
        {
            interactCount--;
            Invoke("InvokeInteractEvent", delayToCall);
        }

        if(interactCount == -1)
            Invoke("InvokeInteractEvent", delayToCall);
    }

    private void InvokeInteractEvent()
    {
        interactEvent?.Invoke();
    }

    private void OnMouseDown()
    {
        if (canClickInteract)
            CallInteract();
    }
}
