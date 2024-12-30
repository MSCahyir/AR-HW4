using UnityEngine;

public class RayBouncer : MonoBehaviour
{
    public Transform rayStart; // Işının başlangıç pozisyonu
    public int maxBounces = 5; // Maksimum sekme sayısı
    public float rayLength = 100f; // Işının uzunluğu
    public LineRenderer lineRenderer; // Çizgi görselleştirme için LineRenderer

    void Start()
    {
        // LineRenderer bileşenini oluştur
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = maxBounces + 1; // Her sekme için bir nokta
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.blue;
        lineRenderer.endColor = Color.cyan;
    }

    void Update()
    {
        Vector3 currentPosition = rayStart.position;
        Vector3 currentDirection = rayStart.forward; // Işının başlangıç yönü

        lineRenderer.SetPosition(0, currentPosition); // İlk nokta başlangıç pozisyonu

        for (int i = 0; i < maxBounces; i++)
        {
            Ray ray = new Ray(currentPosition, currentDirection);
            if (Physics.Raycast(ray, out RaycastHit hit, rayLength))
            {
                // Çarpışma noktasını kaydet
                lineRenderer.SetPosition(i + 1, hit.point);

                // Yansıma yönünü hesapla
                currentDirection = Vector3.Reflect(currentDirection, hit.normal);

                // Yeni pozisyonu güncelle
                currentPosition = hit.point;
            }
            else
            {
                // Eğer ışın bir nesneyle çarpışmazsa, ilerlemeye devam eder
                lineRenderer.positionCount = i + 2;
                lineRenderer.SetPosition(i + 1, currentPosition + currentDirection * rayLength);
                break;
            }
        }
    }
}
