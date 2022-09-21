using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialBehavior : MonoBehaviour
{
    public Material _inactiveMaterial;
    public Material _activeMaterial;

    private MeshRenderer _meshRenderer;
    private bool _toggleState = false;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.material = _inactiveMaterial;
    }

    public void ToggleMaterial()
    {
        _toggleState = !_toggleState;
        if(_toggleState)       
            _meshRenderer.material = _activeMaterial;
        else
            _meshRenderer.material = _inactiveMaterial;
    }
}
