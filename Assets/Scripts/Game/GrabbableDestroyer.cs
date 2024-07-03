using UnityEngine;

public class GrabbableDestroyer : MonoBehaviour
{
    [SerializeField] private AudioClip destroyClip;
    private InstanceCreator creator;

    public void SetCreator(InstanceCreator creater)
    {
        this.creator = creater;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out ClearTrigger clearTrigger))
        {
            if (clearTrigger.GetIsEnable())
                DestroyInstance();
        }
    }

    public void DestroyInstance()
    {
        SoundManager.PlayAudioClip(destroyClip);
        FindFirstObjectByType<Grab_Items>().ClearGrab();
        Destroy(gameObject);
    }
}
