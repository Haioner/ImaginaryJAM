using UnityEngine;

public class HandCameraDirection : MonoBehaviour
{
    [SerializeField] private Transform target;

    void Update()
    {
        float cameraRotationX = target.eulerAngles.x;
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, -cameraRotationX - 107);
    }
}
