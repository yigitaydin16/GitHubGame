using UnityEngine;

public class Footstep : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip roarClip;

    private void Start()
    {
        audioSource = GetComponentInParent<AudioSource>();
        audioSource.clip = roarClip;
        audioSource.loop = true; // Sürekli çalsýn
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
