using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ProceduralTerrain2D : MonoBehaviour
{
    [SerializeField] private int width = 512;
    [SerializeField] private int height = 256;
    [SerializeField] private float scale = 0.05f;
    [SerializeField] private float seed = 0f;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        SetupLineRenderer();
    }

    void Start()
    {
        GenerateTerrain();
    }

    // Allows for regeneration in the editor when values are changed.
    private void OnValidate()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
            SetupLineRenderer();
        }
        GenerateTerrain();
    }

    private void SetupLineRenderer()
    {
        // A material is required for the line to be visible.
        // A simple "Sprites-Default" material works well for unlit 2D lines.
        // This must be set in the inspector.
        if (lineRenderer.sharedMaterial == null)
        {
            // Unity's default material for lines.
            var material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
            lineRenderer.sharedMaterial = material;
        }

        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
        lineRenderer.useWorldSpace = false;
    }

    [ContextMenu("Generate Terrain")]
    public void GenerateTerrain()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
            SetupLineRenderer();
        }

        lineRenderer.positionCount = width;
        Vector3[] points = new Vector3[width];

        // Use the seed to get a different noise pattern
        float yOffset = seed;

        for (int x = 0; x < width; x++)
        {
            float noiseVal = Mathf.PerlinNoise(x * scale, yOffset) * height;
            points[x] = new Vector3(x, noiseVal, 0);
        }

        lineRenderer.SetPositions(points);
    }
}