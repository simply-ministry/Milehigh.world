// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
using UnityEngine;

/// <summary>
/// Generates a 2D terrain profile using Perlin noise and displays it with a LineRenderer.
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class ProceduralTerrain2D : MonoBehaviour
{
    [Header("Terrain Settings")]
    [Tooltip("The number of points to generate for the terrain's width.")]
    [SerializeField] private int width = 512;
    [Tooltip("The maximum height of the terrain.")]
    [SerializeField] private int height = 256;
    [Tooltip("The scale of the Perlin noise. Smaller values create smoother terrain.")]
    [SerializeField] private float scale = 0.05f;
    [Tooltip("The seed for the Perlin noise, allowing for reproducible terrain.")]
    [SerializeField] private float seed = 0f;

    private LineRenderer lineRenderer;

    /// <summary>
    /// Initializes the component by getting the LineRenderer.
    /// </summary>
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        SetupLineRenderer();
    }

    /// <summary>
    /// Generates the terrain when the game starts.
    /// </summary>
    void Start()
    {
        GenerateTerrain();
    }

    /// <summary>
    /// Called in the editor when a script is loaded or a value is changed in the Inspector.
    /// This allows for real-time regeneration of the terrain in the editor.
    /// </summary>
    private void OnValidate()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
            SetupLineRenderer();
        }
        GenerateTerrain();
    }

    /// <summary>
    /// Sets up the initial properties of the LineRenderer.
    /// </summary>
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

    /// <summary>
    /// Generates the terrain points using Perlin noise and applies them to the LineRenderer.
    /// Can be called from a context menu in the editor.
    /// </summary>
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