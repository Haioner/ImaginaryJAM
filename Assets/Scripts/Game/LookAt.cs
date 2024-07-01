using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] private bool canLookAt;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private Transform target;

    private void FixedUpdate()
    {
        if (!canLookAt || target == null) return;

        Vector3 direction = target.position - transform.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }
}
