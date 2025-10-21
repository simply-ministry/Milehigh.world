// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
using UnityEngine;

/// <summary>
/// Controls the launching mechanism for a Golem, including trajectory prediction.
/// This script calculates the path of a projectile and can launch a prefab along that path.
/// </summary>
public class Mptaro : MonoBehaviour
{
    [Header("References")]
    /// <summary>
    /// The target destination for the launch.
    /// </summary>
    public Transform target;
    /// <summary>
    /// The prefab of the Golem to be launched.
    /// </summary>
    public GameObject golemPrefab;
    /// <summary>
    /// The point from which the Golem will be launched.
    /// </summary>
    public Transform spawnPoint;
    /// <summary>
    /// The LineRenderer used to visualize the launch trajectory.
    /// </summary>
    public LineRenderer trajectoryLine;

    [Header("Launch Parameters")]
    /// <summary>
    /// The angle of the launch in degrees.
    /// </summary>
    public float launchAngle = 45f;
    /// <summary>
    /// The initial force applied to the Golem at launch.
    /// </summary>
    public float launchForce = 20f;

    [Header("Trajectory Prediction")]
    [SerializeField] private int trajectorySteps = 100;
    [SerializeField] private float trajectoryTimeStep = 0.1f;

    /// <summary>
    /// Initializes the component, ensuring the LineRenderer is assigned.
    /// </summary>
    void Start()
    {
        if (trajectoryLine == null)
        {
            trajectoryLine = GetComponent<LineRenderer>();
        }
    }

    /// <summary>
    /// Called every frame. Updates the trajectory visualization and listens for the launch input.
    /// </summary>
    void Update()
    {
        if (trajectoryLine != null)
        {
            Vector3[] points = CalculateTrajectoryPoints();
            trajectoryLine.positionCount = points.Length;
            trajectoryLine.SetPositions(points);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Launch();
        }
    }

    /// <summary>
    /// Launches the Golem prefab towards the target.
    /// Instantiates the Golem and applies the calculated launch velocity to its Rigidbody.
    /// </summary>
    public void Launch()
    {
        if (golemPrefab == null || spawnPoint == null || target == null)
        {
            Debug.LogWarning("Mptaro: Missing references for launching.");
            return;
        }

        GameObject golemInstance = Instantiate(golemPrefab, spawnPoint.position, Quaternion.identity);
        Rigidbody rb = golemInstance.GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Mptaro: Golem prefab is missing a Rigidbody component.");
            Destroy(golemInstance);
            return;
        }

        Vector3 direction = (target.position - spawnPoint.position).normalized;
        direction.y = 0;
        Quaternion rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(-launchAngle, 0, 0);
        Vector3 launchVelocity = rotation * Vector3.forward * launchForce;

        rb.velocity = launchVelocity;
    }

    /// <summary>
    /// Calculates the points along the projectile's trajectory for visualization.
    /// </summary>
    /// <returns>An array of Vector3 points representing the trajectory path.</returns>
    private Vector3[] CalculateTrajectoryPoints()
    {
        if (target == null || spawnPoint == null)
        {
            return new Vector3[0];
        }

        Vector3 direction = (target.position - spawnPoint.position).normalized;
        direction.y = 0;
        Quaternion rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(-launchAngle, 0, 0);
        Vector3 startVelocity = rotation * Vector3.forward * launchForce;

        Vector3[] points = new Vector3[trajectorySteps];
        for (int i = 0; i < trajectorySteps; i++)
        {
            float t = i * trajectoryTimeStep;
            points[i] = spawnPoint.position + startVelocity * t + 0.5f * Physics.gravity * t * t;
        }

        return points;
    }
}