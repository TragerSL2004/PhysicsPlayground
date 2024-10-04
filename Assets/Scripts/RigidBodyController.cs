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
    private float _jumpHeight;

    [Space]
    [SerializeField]
    private Animator _animator;

    private float _speed;

    private Rigidbody _rigidBody;

    private Vector2 _moveInput;

    private bool _jumpInput;

    private bool _isIdle;

    private Vector3 _direction;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!_animator)
            return;

        //Ragdoll the player when ragdoll is off and R is pressed
        if (Input.GetKeyDown(KeyCode.R))
            SetRagdollOn();
        //Reset the player when ragdoll is on and T is pressed
        if (Input.GetKeyDown(KeyCode.T))
            SetRagdollOff();


        //Set when animator should do certain animations
        _animator.SetFloat("Speed", _speed);
        _animator.SetFloat("JumpHeight", _jumpHeight);
        _animator.SetBool("Jump", _jumpInput);
        _animator.SetBool("Rest", _isIdle);
    }

    private void FixedUpdate()
    {
        Vector3 force = _direction.normalized * _speed * Time.fixedDeltaTime;

        //If move inputs are received...
        if (_moveInput.magnitude > 0.1f)
        {
            //Set speed of animator and move the player
            _speed = 10;
            _rigidBody.AddForce(force, ForceMode.VelocityChange);
        }
        else if (_jumpInput)
            _speed = 3;
        //If no inputs are being given...
        else
            //Make animator idle
            _speed = 0;

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.action.ReadValue<Vector2>();
        _direction = new Vector3(_moveInput.x, 0, _moveInput.y);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        _jumpInput = context.action.ReadValue<float>() > 0;
        Vector3 jumpForce = new Vector3(0, _jumpHeight, 0);
        _rigidBody.AddForce(jumpForce, ForceMode.Impulse);
    }

    private void SetRagdollOn()
    {
        ToggleRagdoll(true);
    }

    private void SetRagdollOff()
    {
        ToggleRagdoll(false);
    }

    private void ToggleRagdoll(bool enabled)
    {
        _animator.enabled = !enabled;
        _rigidBody.isKinematic = enabled;
        TryGetComponent(out Collider collider);

        if (collider)
            collider.enabled = !enabled;

        foreach (var item in GetComponentsInChildren<Rigidbody>(true))
            if (item != _rigidBody)
                item.isKinematic = !enabled;
        foreach (var item in GetComponentsInChildren<Collider>(true))
            if (item != collider)
                item.enabled = enabled;
    }
}
