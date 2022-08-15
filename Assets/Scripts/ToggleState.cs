using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleState : MonoBehaviour
{
    private bool _state = false;

    public GameObject[] GameObjects;

    public void ToggleButton()
    {
        _state = !_state;
        foreach (var item in GameObjects)
        {
            item.SetActive(_state);
        }
    }
}
