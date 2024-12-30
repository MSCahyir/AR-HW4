using UnityEngine;

public class RayVisualizer : MonoBehaviour
{
    public Transform rayStart; // Işının başlangıç pozisyonu
    public BlackHolePhysics physics; // Kara delik fiziği

    public int maxSteps = 50; // Işının maksimum iterasyon sayısı
    public float stepSize = 0.5f; // Her bir adımın mesafesi
    public LineRenderer lineRenderer;

    void Start()
    {
        // LineRenderer bileşenini oluştur
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = maxSteps; // Maksimum adım kadar nokta
        lineRenderer.startWidth = 0.05f; // Çizginin başlangıç kalınlığı
        lineRenderer.endWidth = 0.05f; // Çizginin bitiş kalınlığı
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red; // Çizgi başlangıç rengi
        lineRenderer.endColor = Color.yellow; // Çizgi bitiş rengi
    }

    void Update()
    {
        Vector3 currentPosition = rayStart.position; // Işının başlangıç pozisyonu
        Vector3 direction = rayStart.forward; // Işının yönü

        for (int i = 0; i < maxSteps; i++)
        {
            lineRenderer.SetPosition(i, currentPosition); // Çizgi üzerinde bir nokta ayarla
            Vector3 nextPosition = physics.CalculateRay(currentPosition, direction); // Işının yeni pozisyonu

            if ((nextPosition - currentPosition).magnitude < stepSize)
            {
                lineRenderer.positionCount = i + 1; // Fazla noktaları kaldır
                break;
            }

            currentPosition = nextPosition; // Pozisyonu güncelle
        }
    }
}
