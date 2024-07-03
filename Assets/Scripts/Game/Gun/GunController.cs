using DG.Tweening;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [Header("Door/Locker")]
    [SerializeField] private LockerInteract currentLocker;
    [SerializeField] private DoorController currentDoor;

    [Header("Gun")]
    [SerializeField] private float cooldownTime = 1f;
    public float shootDistance = 50f;
    [SerializeField] private Transform shootPos;
    [SerializeField] private LayerMask rayLayer;

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
    [SerializeField] private float lineDestroyTimer = 0.05f;
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

        if (Input.GetKeyDown(KeyCode.F))
            Clear();
    }

    public void Clear()
    {
        if (currentDoor == null && currentLocker == null) return;

        ResetLocker();
        ResetDoor();
        anim.SetTrigger("Clear");
        SoundManager.PlayAudioClip(missClip);
        UpdateFeedback();
    }

    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= lastShotTime + cooldownTime)
        {
            anim.SetTrigger("Shoot");
            PlayShootParticle();
            CheckForInteractable();
            lastShotTime = Time.time;
        }
    }

    private void PlayShootParticle()
    {
        Instantiate(shootParticlePrefab, shootPos.position, Quaternion.identity);
    }

    private void PlayHitParticle(Vector3 hitPosition)
    {
        Instantiate(hitParticlePrefab, hitPosition, Quaternion.identity);
    }

    private RaycastHit GetShootHit()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, shootDistance, rayLayer);
        return hit;
    }

    private void CheckForInteractable()
    {
        RaycastHit hit = GetShootHit();

        if (hit.collider != null)
        {
            LockerInteract locker = hit.collider.GetComponent<LockerInteract>();
            DoorController door = hit.collider.GetComponent<DoorController>();

            if (locker != null && locker.canGunConnect)
            {
                ResetLocker();

                currentLocker = locker;
                currentLocker.SetMaterial(1);
                SoundManager.PlayAudioClip(lockerClip);
            }
            else if (door != null && door.canGunConnect)
            {
                ResetDoor();

                currentDoor = door;
                currentDoor.SetMaterial(1);
                SoundManager.PlayAudioClip(doorClip);
            }
            else
            {
                SoundManager.PlayAudioClip(missClip);
            }

            if (currentLocker != null && currentDoor != null)
            {
                currentLocker.SetDoorController(currentDoor);
            }
            UpdateFeedback();
            PlayHitParticle(hit.point);
            DrawLine(shootPos.position, hit.point);
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    public void CallLockerInteract()
    {
        if (currentLocker == null) return;
        currentLocker.Interact();
    }

    private void ResetLocker()
    {
        if (currentLocker != null)
        {
            currentLocker.SetDoorController(null);
            currentLocker.SetMaterial(0);
            currentLocker = null;
        }
    }

    private void ResetDoor()
    {
        if (currentDoor != null)
        {
            currentDoor.SetMaterial(0);
            currentDoor = null;
        }
    }

    private void UpdateFeedback()
    {
        if (currentLocker != null || currentDoor != null)
        {
            cylinderMesh.material = cylinderMat[1];
            cylinderDOT.DORestart();
        }
        else
        {
            cylinderMesh.material = cylinderMat[0];
            cylinderDOT.DOPause();
        }
    }

    private void DrawLine(Vector3 start, Vector3 end)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        lineRenderer.enabled = true;
        Invoke("DisableLine", lineDestroyTimer);
    }

    private void DisableLine()
    {
        lineRenderer.enabled = false;
    }
}
