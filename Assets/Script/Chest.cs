using UnityEngine;

public class ChestOpener : MonoBehaviour
{
    private Animator anim;
    private bool hasOpened = false;

    [Header("Item Spawn")]
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform spawnPoint;

    [Header("Audio")]
    [SerializeField] private AudioClip openSound;
    private AudioSource audioSource;

    void Start()
    {
        anim = GetComponentInParent<Animator>();
        audioSource = GetComponent<AudioSource>(); // Ayn� objeye AudioSource eklersen �al���r
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasOpened && other.CompareTag("Player"))
        {
            hasOpened = true;

            // SES� �AL
            if (openSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(openSound);
            }

            // AN�MASYONU BA�LAT
            anim.SetTrigger("OpenChest");

            // ITEM SPAWN
            Invoke(nameof(SpawnItem), 0.5f);
        }
    }

    private void SpawnItem()
    {
        if (itemPrefab != null && spawnPoint != null)
        {
            Instantiate(itemPrefab, spawnPoint.position, Quaternion.identity);
        }
    }
}
