using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPoseObjectBehavior : MonoBehaviour
{
    public MeshRenderer MeshRenderer;
    public GameObject SafetySphere;
    public float OutOfBoundsRadius = 0.8f;

    private Vector3 _position;

    public ChangeMaterialColor ChangeColor;


    private void Start()
    {
        MeshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        _position = this.transform.position;

        if(Vector3.Distance(SafetySphere.transform.position, _position) > OutOfBoundsRadius)
        {
            ChangeColor.OutofBoundsColor();
        }
        else
        {
            ChangeColor.CheckforInBounds();
        }
    }
}
