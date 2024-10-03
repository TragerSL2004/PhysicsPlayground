using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class RigidBodyController : MonoBehaviour
{
    [SerializeField]
    private float m_jumpHeight;
 
    [SerializeField]
    private Animator m_animator;

    private float m_speed;

    private Rigidbody m_rigidBody;

    private Vector2 moveInput;

    private bool jumpInput;

    private bool isIdle;

    private Vector3 direction;

    private void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();

    }

    private void Update()
    {
        if (!m_animator)
            return;

        if (Input.GetKeyDown(KeyCode.R))
            SetRagdollOn();

        m_animator.SetFloat("Speed", m_speed);
        m_animator.SetFloat("JumpHeight", m_jumpHeight);
        m_animator.SetBool("Jump", jumpInput);
        m_animator.SetBool("Rest", isIdle);
    }

    private void FixedUpdate()
    {
        Vector3 force = direction.normalized * m_speed * Time.fixedDeltaTime;

        if (moveInput.magnitude > 0.1f)
        {
            m_speed = 10;
            m_rigidBody.AddForce(force, ForceMode.VelocityChange);
        }
        else if (jumpInput)
            m_speed = 3;
        else
            m_speed = 0;

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.action.ReadValue<Vector2>();
        direction = new Vector3(moveInput.x, 0, moveInput.y);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        jumpInput = context.action.ReadValue<float>() > 0;
        Vector3 jumpForce = new Vector3(0, m_jumpHeight, 0);
        m_rigidBody.AddForce(jumpForce, ForceMode.Impulse);
    }

    private void SetRagdollOn()
    {
        ToggleRagdoll(true);
    }

    private void ToggleRagdoll(bool enabled)
    {
        m_animator.enabled = false;
        m_rigidBody.isKinematic = enabled;
        TryGetComponent(out Collider collider);

        if (collider)
            collider.enabled = !enabled;

        foreach (var item in GetComponentsInChildren<Rigidbody>(true))
            if (item != m_rigidBody)
                item.isKinematic = !enabled;
        foreach (var item in GetComponentsInChildren<Collider>(true))
            if (item != collider)
                item.enabled = enabled;
    }
}
