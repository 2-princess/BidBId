using UnityEngine;

public class WaterScroll : MonoBehaviour
{
    [SerializeField] private Renderer waterRenderer;
    [SerializeField] private float speedX = 0.02f;
    [SerializeField] private float speedY = 0.01f;

    private Material waterMat;
    private Vector2 offset;

    void Start()
    {
        if (waterRenderer == null)
            waterRenderer = GetComponent<Renderer>();

        waterMat = waterRenderer.material;
    }

    void Update()
    {
        offset.x += speedX * Time.deltaTime;
        offset.y += speedY * Time.deltaTime;

        waterMat.SetTextureOffset("_BaseMap", offset);
    }
}