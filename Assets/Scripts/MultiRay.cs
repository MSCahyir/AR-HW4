using System.Collections.Generic;
using UnityEngine;

public class MultiRayBouncer : MonoBehaviour
{
    public Transform[] rayStarts = new Transform[3]; // Işınların başlangıç pozisyonları
    public Transform blackHole;                      // Kara delik pozisyonu
    public BlackHolePhysics physics;                 // Kara delik fiziği

    public float blackHoleRadius = 5f;               // Kara deliğin etki alanı
    public int rayCount = 10;                        // Oluşturulacak ışın sayısı
    public int maxBounces = 5;                       // Maksimum sekme sayısı
    public float rayLength = 100f;                   // Işın uzunluğu
    public LineRenderer lineRendererPrefab;          // LineRenderer için prefab
    public int maxSteps = 10;                        // Maksimum adım sayısı

    public bool isBlackHoleActive = true;            // Kara delik aktif mi?

    private List<LineRenderer> lineRenderers = new List<LineRenderer>();
    private List<Vector3> directions = new List<Vector3>(); // Her ışın için yönler
    private List<Vector3> blackHoleOffsets = new List<Vector3>();

    void Start()
    {
        for (int i = 0; i < rayCount; i++)
        {
            Vector3 randomDirection = (Random.onUnitSphere + Random.insideUnitSphere * 0.3f).normalized;
            Vector3 randomOffset = Random.insideUnitSphere * 0.5f;
            blackHoleOffsets.Add(randomOffset);

            for (int j = 0; j < rayStarts.Length; j++)
            {
                LineRenderer lineRenderer = Instantiate(lineRendererPrefab, transform);
                lineRenderer.positionCount = 1;

                lineRenderers.Add(lineRenderer);
                directions.Add(randomDirection);
            }
        }
    }

    void Update()
    {
        int rayIndex = 0;
        for (int j = 0; j < rayStarts.Length; j++)
        {
            for (int i = 0; i < rayCount; i++)
            {
                LineRenderer lineRenderer = lineRenderers[rayIndex];
                Vector3 direction = directions[rayIndex];
                Vector3 currentPosition = rayStarts[j].position;

                directions[rayIndex] = direction;

                List<Vector3> positions = TraceRay(currentPosition, directions[rayIndex], isBlackHoleActive, rayIndex);

                lineRenderer.positionCount = positions.Count;
                lineRenderer.SetPositions(positions.ToArray());

                rayIndex++;
            }
        }
    }

    List<Vector3> TraceRay(Vector3 start, Vector3 direction, bool applyGravity, int rayIndex)
    {
        Vector3 currentPosition = start;
        Vector3 currentDirection = direction;

        List<Vector3> positions = new List<Vector3>();
        positions.Add(currentPosition);

        for (int i = 0; i < maxBounces; i++)
        {
            if (applyGravity && physics.blackHole != null)
            {
                currentDirection = ApplyBlackHoleGravity(currentPosition, currentDirection, rayIndex);
            }

            Ray ray = new Ray(currentPosition, currentDirection);

            if (Physics.Raycast(ray, out RaycastHit hit, rayLength))
            {
                positions.Add(hit.point);
                currentDirection = Vector3.Reflect(currentDirection, hit.normal);
                currentPosition = hit.point;

                if (isBlackHoleActive)
                {
                    positions.Add(physics.blackHole.position);
                    break;
                }
            }
            else
            {
                if (isBlackHoleActive)
                {
                    positions.Add(physics.blackHole.position);
                    break;
                }

                Vector3 nextPosition = currentPosition + currentDirection * rayLength;
                positions.Add(nextPosition);
                break;
            }
        }

        return positions;
    }

    Vector3 ApplyBlackHoleGravity(Vector3 position, Vector3 direction, int rayIndex)
    {
        if (blackHole == null) return direction;

        Vector3 offset = blackHoleOffsets[rayIndex % rayCount];
        Vector3 nextPosition = physics.CalculateRay(position, direction); // Işının yeni pozisyonu

        if ((nextPosition - position).magnitude < 0.5f)
        {
            return direction; // Fazla noktaları kaldır
        }

        return (nextPosition - position).normalized; // Pozisyonu güncelle
    }
}