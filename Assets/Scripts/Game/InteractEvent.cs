using UnityEngine;
using UnityEngine.Events;

public class InteractEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent interactEvent;

    public void CallInteract()
    {
        interactEvent?.Invoke();
    }

    private void OnMouseDown()
    {
        CallInteract();
    }
}
