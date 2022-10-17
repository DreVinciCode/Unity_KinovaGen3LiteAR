using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColor : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = Color.yellow;
    }

    public void ConnectedColor()
    {
        _spriteRenderer.color = Color.green;
    }

    public void DisconnectedColor()
    {
        _spriteRenderer.color = Color.red;
    }


}
