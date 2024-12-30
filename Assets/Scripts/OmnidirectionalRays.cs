using UnityEngine;

public class OmnidirectionalRays : MonoBehaviour
{
    public Transform lightSource; // Işık kaynağı
    public int rayCount = 100; // Yayılacak ışın sayısı
    public float rayLength = 10f; // Işın uzunluğu
    public LineRenderer lineRendererPrefab; // Işınları göstermek için LineRenderer prefab'ı
    public bool useRandomDirections = true; // Rastgele yönler mi kullanılacak?

    void Start()
    {
        for (int i = 0; i < rayCount; i++)
        {
            // Işın yönünü hesapla
            Vector3 direction = useRandomDirections ? Random.onUnitSphere : GetDirection(i);

            // Işını çizmek için bir LineRenderer oluştur
            LineRenderer lineRenderer = Instantiate(lineRendererPrefab, transform);
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, lightSource.position); // Başlangıç noktası
            lineRenderer.SetPosition(1, lightSource.position + direction * rayLength); // Bitiş noktası
        }
    }

    // Düzenli yönler için hesaplama
    Vector3 GetDirection(int index)
    {
        float phi = Mathf.Acos(1 - 2 * (float)index / rayCount); // Polar açı
        float theta = Mathf.PI * (1 + Mathf.Sqrt(5)) * index;   // Altın açı (golden angle)
        float x = Mathf.Sin(phi) * Mathf.Cos(theta);
        float y = Mathf.Sin(phi) * Mathf.Sin(theta);
        float z = Mathf.Cos(phi);
        return new Vector3(x, y, z);
    }
}
