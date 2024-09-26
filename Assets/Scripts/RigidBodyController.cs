using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class RigidBodyController : MonoBehaviour
{
    [SerializeField]
    private float m_speed;

    private Rigidbody m_rigidBody;

    private Vector2 moveInput;

    private Vector3 direction;

    private void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 force = direction.normalized * m_speed * Time.fixedDeltaTime;
        m_rigidBody.AddForce(force, ForceMode.Impulse);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.action.ReadValue<Vector2>();
        direction = new Vector3(moveInput.x, 0, moveInput.y);
    }
}
