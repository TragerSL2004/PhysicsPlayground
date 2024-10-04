using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MeshRenderer))]
public class PhysicsObject : MonoBehaviour
{
    [SerializeField]
    private Material _awakeMaterial;
    [SerializeField]
    private Material _asleepMaterial;

    private Rigidbody _rigidBody;
    private MeshRenderer _meshRenderer;
    private bool _wasSleeping = true;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void FixedUpdate()
    {
        UpdateMeshRenderer();
    }

    private void UpdateMeshRenderer()
    {
        //Check if an object is moving and change color accordingly
        if (_rigidBody.IsSleeping() && !_wasSleeping && _asleepMaterial != null)
        {
            _wasSleeping = true;
            _meshRenderer.material = _asleepMaterial;
        }
        if (!_rigidBody.IsSleeping() && _wasSleeping && _awakeMaterial != null)
        {
            _wasSleeping = false;
            _meshRenderer.material = _awakeMaterial;
        }
    }
}
