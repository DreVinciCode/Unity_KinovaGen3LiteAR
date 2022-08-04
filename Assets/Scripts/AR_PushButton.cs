using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AR_PushButton : MonoBehaviour
{

    public static event Action<string> ARButtonPressed = delegate { };
    private int _dividerPosition;
    private string _buttonName, _buttonValue;

    private void Start()
    {
        _buttonName = gameObject.transform.parent.name;
        _dividerPosition = _buttonName.IndexOf("_");
        _buttonValue = _buttonName.Substring(0, _dividerPosition);
    }

    public void ARButtonClicked()
    {
        ARButtonPressed(_buttonValue);
    }
}
