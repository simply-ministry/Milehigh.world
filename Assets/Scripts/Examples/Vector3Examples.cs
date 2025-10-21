using UnityEngine;

/// <summary>
/// A script demonstrating various common uses of the Vector3 struct in Unity.
/// </summary>
public class Vector3Examples : MonoBehaviour
{
    [Header("Targeting")]
    [Tooltip("Assign a target GameObject in the Inspector to demonstrate targeting behaviors.")]
    public Transform target;
    [Tooltip("The speed at which this object moves towards the target.")]
    public float moveSpeed = 5f;
    [Tooltip("The speed at which this object rotates to face the target.")]
    public float rotationSpeed = 10f;

    [Header("Linear Interpolation (Lerp)")]
    [Tooltip("The starting point for the Lerp demonstration.")]
    public Transform startMarker;
    [Tooltip("The ending point for the Lerp demonstration.")]
    public Transform endMarker;
    [Tooltip("The speed of the Lerp movement.")]
    public float lerpSpeed = 1.0F;
    private float startTime;
    private float journeyLength;

    /// <summary>
    /// Initializes the Lerp example by recording the start time and journey length.
    /// </summary>
    void Start()
    {
        if (startMarker != null && endMarker != null)
        {
            startTime = Time.time;
            journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
        }
    }

    /// <summary>
    /// Called every frame. Executes the Vector3 demonstrations.
    /// </summary>
    void Update()
    {
        // --- Targeting Examples ---
        if (target != null)
        {
            // 1. Moving an object towards a target:
            Vector3 targetDirection = (target.position - transform.position).normalized;
            transform.position += targetDirection * moveSpeed * Time.deltaTime;

            // 2. Rotating to face a target:
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // 3. Calculating distance:
            float distance = Vector3.Distance(transform.position, target.position);
            Debug.Log("Distance to target: " + distance);
        }

        // --- Physics Example ---
        // 4. Creating a force vector and applying it to a Rigidbody:
        if (GetComponent<Rigidbody>() != null)
        {
            Vector3 force = new Vector3(0f, 10f, 0f); // An upward force
            GetComponent<Rigidbody>().AddForce(force * Time.deltaTime);
        }

        // --- Lerp Example ---
        // 5. Moving an object from a start to an end point over time:
        if (startMarker != null && endMarker != null)
        {
            float distCovered = (Time.time - startTime) * lerpSpeed;
            float fractionOfJourney = journeyLength > 0 ? distCovered / journeyLength : 0;
            transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fractionOfJourney);
        }
    }
}