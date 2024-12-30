using UnityEngine;

public class DynamicRayBouncer : MonoBehaviour
{
    public Transform rayStart; // Işının başlangıç pozisyonu
    public int maxBounces = 5; // Maksimum sekme sayısı
    public float rayLength = 100f; // Işın uzunluğu
    public LineRenderer lineRenderer; // Işını çizmek için LineRenderer

    void Start()
    {
        // LineRenderer bileşenini oluştur
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.blue;
        lineRenderer.endColor = Color.cyan;
    }

    void Update()
    {
        Vector3 currentPosition = rayStart.position;
        Vector3 currentDirection = rayStart.forward; // Işının başlangıç yönü

        // Çizgi pozisyonlarını güncellemek için bir liste oluştur
        System.Collections.Generic.List<Vector3> positions = new System.Collections.Generic.List<Vector3>();
        positions.Add(currentPosition);

        for (int i = 0; i < maxBounces; i++)
        {
            // Işını fırlat ve çarpışmayı kontrol et
            Ray ray = new Ray(currentPosition, currentDirection);
            if (Physics.Raycast(ray, out RaycastHit hit, rayLength))
            {
                // Çarpışma noktasını kaydet
                positions.Add(hit.point);

                // Yansıma yönünü hesapla
                currentDirection = Vector3.Reflect(currentDirection, hit.normal);

                // Yeni pozisyonu güncelle
                currentPosition = hit.point;
            }
            else
            {
                // Eğer ışın bir nesneyle çarpışmazsa, çizgiyi devam ettir
                positions.Add(currentPosition + currentDirection * rayLength);
                break;
            }
        }

        // LineRenderer'da pozisyonları güncelle
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }
}
