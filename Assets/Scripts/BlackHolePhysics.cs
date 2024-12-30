using UnityEngine;

public class BlackHolePhysics : MonoBehaviour
{
    public Transform blackHole;
    public float blackHoleRadius = 5f;

    public Vector3 CalculateRay(Vector3 origin, Vector3 direction)
    {
        // Kara deliğe olan mesafe
        Vector3 toBlackHole = blackHole.position - origin;
        float distance = toBlackHole.magnitude;

        if (distance < blackHoleRadius)
        {
            // Kara deliğe ulaşıldı
            return blackHole.position;
        }

        // Eğrilik etkisi (derece 2 eğri)
        Vector3 curve = direction + toBlackHole.normalized * Mathf.Pow(distance / blackHoleRadius, 2);
        return origin + curve.normalized * distance;
    }
}
