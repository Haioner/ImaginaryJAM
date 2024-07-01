using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [Header("CACHE")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Camera cam;
    [SerializeField] private List<Camera> camerasToDisable = new List<Camera>();

    [Header("Zoom")]
    [SerializeField] private float currentZoomAmount = 30f;
    [SerializeField] private float modifierAmount = 3f;
    [SerializeField] private float maxZoomAmount = 20f;
    [SerializeField] private float zoomVelocity = 7f;

    private float _initialZoomAmount;
    private float _initialCamFov;
    private DefaultInput _defaultInput;

    private void Awake()
    {
        _defaultInput = new DefaultInput();
        _defaultInput.View.ZoomScrollUp.performed += e => AddZoom();
        _defaultInput.View.ZoomScrollDown.performed += e => SubtractZoom();
        _defaultInput.Enable();

        _initialZoomAmount = currentZoomAmount;
        _initialCamFov = cam.fieldOfView;
    }

    private void Update() => Zoom();

    private void Zoom()
    {
        if (_defaultInput.View.Zoom.ReadValue<float>() != 0 && !playerController.IsSprinting)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, currentZoomAmount, Time.deltaTime * zoomVelocity);

            //Disable Cameras
            for (int i = 0; i < camerasToDisable.Count; i++)
            {
                if (camerasToDisable[i].enabled)
                    camerasToDisable[i].enabled = false;
            }
        }
        else
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, _initialCamFov, Time.deltaTime * zoomVelocity);
            currentZoomAmount = _initialZoomAmount;

            //Enable Cameras
            for (int i = 0; i < camerasToDisable.Count; i++)
            {
                if (!camerasToDisable[i].enabled)
                    camerasToDisable[i].enabled = true;
            }
        }
    }

    private void AddZoom()
    {
        if (currentZoomAmount > maxZoomAmount)
            currentZoomAmount -= modifierAmount;

        if (currentZoomAmount < maxZoomAmount)
            currentZoomAmount = maxZoomAmount;
    }

    private void SubtractZoom()
    {
        float minZoom = _initialCamFov - 10;

        if (currentZoomAmount < minZoom)
            currentZoomAmount += modifierAmount;

        if (currentZoomAmount > minZoom)
            currentZoomAmount = minZoom;
    }
}
