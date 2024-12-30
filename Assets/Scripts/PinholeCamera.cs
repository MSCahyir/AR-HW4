using UnityEngine;

public class PinholeCamera : MonoBehaviour
{
    public int imageWidth = 640;
    public int imageHeight = 480;
    public float fieldOfView = 60f;

    private Camera camera;

    void Start()
    {
        camera = GetComponent<Camera>();
        camera.enabled = true; // Standart kamera özelliklerini devre dışı bırakıyoruz
    }

    public Ray GenerateRay(float x, float y)
    {
        // Kamera koordinatlarını normalize ediyoruz
        float aspectRatio = (float)imageWidth / imageHeight;
        float px = (2 * (x + 0.5f) / imageWidth - 1) * Mathf.Tan(fieldOfView * 0.5f * Mathf.Deg2Rad) * aspectRatio;
        float py = (1 - 2 * (y + 0.5f) / imageHeight) * Mathf.Tan(fieldOfView * 0.5f * Mathf.Deg2Rad);

        Vector3 rayDirection = new Vector3(px, py, 1).normalized; // Işın yönü
        return new Ray(transform.position, transform.TransformDirection(rayDirection));
    }
}
