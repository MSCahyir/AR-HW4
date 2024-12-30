using UnityEngine;

public class RayCaster : MonoBehaviour
{
    public PinholeCamera camera;
    public BlackHolePhysics physics;
    public Texture2D renderTexture;

    void Start()
    {
        renderTexture = new Texture2D(camera.imageWidth, camera.imageHeight);
        RenderScene();
    }

    void RenderScene()
    {
        for (int y = 0; y < camera.imageHeight; y++)
        {
            for (int x = 0; x < camera.imageWidth; x++)
            {
                Ray ray = camera.GenerateRay(x, y);
                Vector3 color = TraceRay(ray);
                renderTexture.SetPixel(x, y, new Color(color.x, color.y, color.z));
            }
        }
        renderTexture.Apply();
    }

    Vector3 TraceRay(Ray ray)
    {
        Vector3 hitPoint = physics.CalculateRay(ray.origin, ray.direction);
        return hitPoint.magnitude < physics.blackHoleRadius ? new Vector3(0, 0, 0) : new Vector3(1, 1, 1);
    }
}
