using UnityEngine;

public class Vector3Examples : MonoBehaviour
{
    public Transform target; // Assign a target Game bo7Object in the Inspector
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    public Transform startMarker;
    public Transform endMarker;
    public float lerpSpeed = 1.0F;
    private float startTime;
    private float journeyLength;

    void Start()
    {
        // For Lerp
        if (startMarker != null && endMarker != null)
        {
            startTime = Time.time;
            journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
        }
    }

    void Update()
    {
        // 1. Moving an object towards a target:
        if (target != null)
        {
            Vector3 targetDirection = (target.position - transform.position).normalized;
            transform.position += targetDirection * moveSpeed * Time.deltaTime;

            // 2. Rotating to face a target:
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // 3. Calculating distance:
            float distance = Vector3.Distance(transform.position, target.position);
            Debug.Log("Distance to target: " + distance);
        }

        // 4. Creating a force vector:
        Vector3 force = new Vector3(2f, 5f, 0f); // Force with x=2, y=5, z=0
        // (Apply force to a Rigidbody component)
        if (GetComponent<Rigidbody>() != null)
        {
            GetComponent<Rigidbody>().AddForce(force);
        }

        // 5. Example of Lerp (Linear Interpolation)
        if (startMarker != null && endMarker != null)
        {
            float distCovered = (Time.time - startTime) * lerpSpeed;
            float fractionOfJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fractionOfJourney);
        }
    }
}