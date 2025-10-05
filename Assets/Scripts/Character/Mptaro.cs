using UnityEngine;

public class Mptaro : MonoBehaviour
{
    [Header("References")]
    public Transform target;
    public GameObject golemPrefab;
    public Transform spawnPoint;
    public LineRenderer trajectoryLine;

    [Header("Launch Parameters")]
    public float launchAngle = 45f;
    public float launchForce = 20f;

    // Start is called before the first frame update
    void Start()
    {
        if (trajectoryLine == null)
        {
            trajectoryLine = GetComponent<LineRenderer>();
        }
    }

    [Header("Trajectory Prediction")]
    [SerializeField] private int trajectorySteps = 100;
    [SerializeField] private float trajectoryTimeStep = 0.1f;

    // Update is called once per frame
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