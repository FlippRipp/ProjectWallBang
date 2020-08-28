using UnityEngine;

public class AntiGravity : MonoBehaviour
{
    private Rigidbody body;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        body.AddForce(-Physics.gravity * body.mass);
    }
}
