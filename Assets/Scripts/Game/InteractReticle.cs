using UnityEngine;
using UnityEngine.UI;

public class InteractReticle : MonoBehaviour
{
    [SerializeField] private Image reticleImage;
    [SerializeField] private Sprite defaultReticle;
    [SerializeField] private Sprite interactableReticle;
    [SerializeField] private Sprite grabReticle;
    [SerializeField] private LayerMask grabbableLayer;
    [SerializeField] private float maxRaycastDistance = 3f;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        ChangeReticle();
    }

    private void ChangeReticle()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxRaycastDistance))
        {
            GameObject hitObject = hit.collider.gameObject;
            bool changingReticle = false;

            if (hitObject.GetComponent<IInteractable>() != null)
            {
                reticleImage.sprite = interactableReticle;
                changingReticle = true;
            }
            if (((1 << hitObject.layer) & grabbableLayer) != 0)
            {
                reticleImage.sprite = grabReticle;
                changingReticle = true;
            }
            if (!changingReticle)
            {
                reticleImage.sprite = defaultReticle;
            }

        }
        else
        {
            reticleImage.sprite = defaultReticle;
        }
    }
}
