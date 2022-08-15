
using UnityEngine;
using UnityEngine.Events;

public class ToggleState : MonoBehaviour
{
    [System.Serializable]
    public class ToggleButtonClickCallBack : UnityEvent<bool> { };

    public ToggleButtonClickCallBack OnToggle = new ToggleButtonClickCallBack();

    private bool _state = false;

    public void CallOnToggle()
    {
        _state = !_state;
        OnToggle.Invoke(_state);
    }
}
