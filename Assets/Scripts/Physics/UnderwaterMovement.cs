using UnityEngine;

public class UnderwaterMovement : MonoBehaviour
{
    public float swimSpeed = 3.0f;
    public float verticalSwimSpeed = 2.0f;
    public float buoyancy = 1.5f; // Strength of upward force
    public bool isUnderwater = false;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
        }
    }

    void FixedUpdate()
    {
        if (isUnderwater)
        {
            // Buoyancy
            rb.AddForce(Vector3.up * buoyancy, ForceMode.Acceleration);

            // Swim input
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            float ascend = 0f;

            if (Input.GetKey(KeyCode.Space)) ascend = 1f;
            if (Input.GetKey(KeyCode.LeftControl)) ascend = -1f;

            Vector3 swimDirection = new Vector3(h, ascend, v).normalized;
            rb.AddForce(swimDirection * swimSpeed, ForceMode.Acceleration);
        }
    }
}