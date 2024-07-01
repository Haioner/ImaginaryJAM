using DG.Tweening;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [Header("Door/Locker")]
    [SerializeField] private LockerInteract currentLocker;
    [SerializeField] private DoorController currentDoor;

    [Header("Gun")]
    [SerializeField] private float cooldownTime = 1f;
    [SerializeField] private float shootDistance = 5f;

    [Header("Feedback")]
    [SerializeField] private Material[] cylinderMat;
    [SerializeField] private MeshRenderer cylinderMesh;
    [SerializeField] private DOTweenAnimation cylinderDOT;

    [Header("Audios")]
    [SerializeField] private AudioClip lockerClip;
    [SerializeField] private AudioClip doorClip;
    [SerializeField] private AudioClip missClip;

    [Header("Particles")]
    [SerializeField] private GameObject shootParticlePrefab;
    [SerializeField] private GameObject hitParticlePrefab;

    [Header("Line")]
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Color lineColor;

    private float lastShotTime;
    private Camera mainCamera;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        mainCamera = Camera.main;
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        lineRenderer.enabled = false;
    }

    private void Update()
    {
        Shoot();
    }

    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= lastShotTime + cooldownTime)
        {
            anim.SetTrigger("Shoot");
            PlayShootParticle();
            CheckForInteractable();
            lastShotTime = Time.time;
            Debug.Log(GetShootHit().collider.name);
        }
    }

    private void PlayShootParticle()
    {
        Vector3 shootPosition = transform.position; // Adjust this to the correct position where the gun shoots from
        Instantiate(shootParticlePrefab, shootPosition, Quaternion.identity);
    }

    private void PlayHitParticle(Vector3 hitPosition)
    {
        Instantiate(hitParticlePrefab, hitPosition, Quaternion.identity);
    }

    private RaycastHit GetShootHit()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, shootDistance);
        return hit;
    }

    private void CheckForInteractable()
    {
        RaycastHit hit = GetShootHit();

        if (hit.collider != null)
        {
            LockerInteract locker = hit.collider.GetComponent<LockerInteract>();
            DoorController door = hit.collider.GetComponent<DoorController>();

            if (locker != null)
            {
                currentLocker = locker;
                SoundManager.PlayAudioClip(lockerClip);
            }
            else if (door != null)
            {
                currentDoor = door;
                SoundManager.PlayAudioClip(doorClip);
            }
            else
            {
                SoundManager.PlayAudioClip(missClip);
            }

            if (currentLocker != null && currentDoor != null)
            {
                currentLocker.SetDoorController(currentDoor);
                currentLocker = null;
                currentDoor = null;
            }
            UpdateFeedback();
            PlayHitParticle(hit.point);
            DrawLine(transform.position, hit.point);
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    private void UpdateFeedback()
    {
        if (currentLocker != null)
        {
            cylinderMesh.material = cylinderMat[1];
            cylinderDOT.enabled = true;
        }
        else
        {
            cylinderMesh.material = cylinderMat[0];
            cylinderDOT.enabled = false;
        }
    }

    private void DrawLine(Vector3 start, Vector3 end)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        lineRenderer.enabled = true;
        Invoke("DisableLine", 0.1f); // Adjust the duration as needed
    }

    private void DisableLine()
    {
        lineRenderer.enabled = false;
    }
}
