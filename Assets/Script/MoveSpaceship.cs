using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpaceship : MonoBehaviour
{
  // Inspector'dan ayarlanabilecek değişkenler
    [Tooltip("Uzay aracının hareket hızı.")]
    public float moveSpeed = 2f; 

    [Tooltip("Uzay aracının başlangıç noktasından sağa veya sola gideceği maksimum mesafe.")]
    public float moveDistance = 5f; 

    private Vector3 startPosition; // Uzay aracının başlangıç konumunu tutar

    void Start()
    {
        // Oyun başladığında uzay aracının mevcut konumunu kaydet
        startPosition = transform.position; 
    }

    void Update()
    {
        // Her karede çalışır ve uzay aracının konumunu günceller

        // Mathf.PingPong, zamanla birlikte değeri 0 ile (moveDistance * 2) arasında ileri geri hareket ettirir.
        // Sonuçtan moveDistance çıkararak hareket aralığını -moveDistance ile +moveDistance arasına taşıyoruz.
        float horizontalMovement = Mathf.PingPong(Time.time * moveSpeed, moveDistance * 2) - moveDistance;

        // Uzay aracının yeni konumunu hesapla.
        // Başlangıç konumuna, sadece yatay (x ekseni) hareketi ekliyoruz.
        // Y ve Z eksenleri sabit kalır.
        transform.position = startPosition + new Vector3(horizontalMovement, 0, 0);
    }
}
