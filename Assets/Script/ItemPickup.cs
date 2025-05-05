using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private float volume = 100.0f; // 1.0'dan büyük yapabilirsin

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && pickupSound != null)
        {
            GameObject tempAudio = new GameObject("TempAudio");
            AudioSource source = tempAudio.AddComponent<AudioSource>();

            source.clip = pickupSound;
            source.volume = volume;
            source.Play();

            Destroy(tempAudio, pickupSound.length);
            Destroy(gameObject);
        }
    }
}
