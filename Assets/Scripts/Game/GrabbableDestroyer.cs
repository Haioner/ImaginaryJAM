using UnityEngine;

public class GrabbableDestroyer : MonoBehaviour
{
    [SerializeField] private AudioClip destroyClip;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<ClearTrigger>() != null)
        {
            SoundManager.PlayAudioClip(destroyClip);
            FindFirstObjectByType<Grab_Items>().ClearGrab();
            Destroy(gameObject);
        }
    }
}
