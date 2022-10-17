using UnityEngine;

public class ChangeMaterialColor : MonoBehaviour
{
    public MeshRenderer MeshRenderer;

    private void Start()
    {
        MeshRenderer = GetComponent<MeshRenderer>();
        MeshRenderer.material.color = Color.yellow;
    }
    public void ManipulationColor()
    {
        MeshRenderer.material.color = Color.white;
    }

    public void EndManipulationColor()
    {
        MeshRenderer.material.color = Color.yellow;
    }
    
    public void OutofBoundsColor()
    {
        MeshRenderer.material.color = Color.red;
    }

    public void CheckforInBounds()
    {
        if(MeshRenderer.material.color == Color.red)
        {
            ManipulationColor();
        }
    }    

}
