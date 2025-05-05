using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private Vector3 startPosition;
    private Transform cam;
    private Vector3 camStartPosition;

    [SerializeField] private float parallaxEffect = 0.5f;

    private void Start()
    {
        cam = Camera.main.transform;
        startPosition = transform.position;
        camStartPosition = cam.position;
    }

    private void LateUpdate()
    {
        Vector3 camDelta = cam.position - camStartPosition;

        transform.position = new Vector3(
            startPosition.x + camDelta.x * parallaxEffect,
            startPosition.y + camDelta.y * parallaxEffect,
            startPosition.z
        );
    }
}
