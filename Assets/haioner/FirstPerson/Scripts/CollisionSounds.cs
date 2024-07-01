using UnityEngine;

public class CollisionSounds : MonoBehaviour
{
    [SerializeField] private AudioClip collisionSound;
    private AudioSource audioSource;

    void Start() => audioSource = GetComponent<AudioSource>();

    void OnCollisionEnter(Collision collision)
    {
        //Calculate impact velocity sound
        float relativeSpeed = collision.relativeVelocity.magnitude;
        float volume = Mathf.Clamp01(relativeSpeed / 10f);
        audioSource.volume = volume;

        if (collisionSound != null)
            audioSource.PlayOneShot(collisionSound);
    }
}
