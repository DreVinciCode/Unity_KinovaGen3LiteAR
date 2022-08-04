using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class AR_Numpad : MonoBehaviour
{
    public TMP_Text IPDisplay;
    public UnityEvent OnAccessGranted;

    private string _pinCode = "";
    private string _code = "1234";

    private void Start()
    {
        IPDisplay.text = _pinCode;
        AR_PushButton.ARButtonPressed += AddDigitToPinSequence;
    }

    private void AddDigitToPinSequence(string digitsEntered)
    {
        if(_pinCode.Length > 3)
        {
            _pinCode = _pinCode.Substring(0, _pinCode.Length - 1);
        }

        switch (digitsEntered)
        {
            case "0":
                _pinCode += "0";
                IPDisplay.text = _pinCode;
                break;

            case "1":
                _pinCode += "1";
                IPDisplay.text = _pinCode;
                break;
            case "2":
                _pinCode += "2";
                IPDisplay.text = _pinCode;
                break;
            case "3":
                _pinCode += "3";
                IPDisplay.text = _pinCode;
                break;
            case "4":
                _pinCode += "4";
                IPDisplay.text = _pinCode;
                break;
            case "5":
                _pinCode += "5";
                IPDisplay.text = _pinCode;
                break;
            case "6":
                _pinCode += "6";
                IPDisplay.text = _pinCode;
                break;
            case "7":
                _pinCode += "7";
                IPDisplay.text = _pinCode;
                break;
            case "8":
                _pinCode += "8";
                IPDisplay.text = _pinCode;
                break;
            case "9":
                _pinCode += "9";
                IPDisplay.text = _pinCode;
                break;
            case ".":
                _pinCode += ".";
                IPDisplay.text = _pinCode;
                break;
            case "x":
                if (_pinCode.Length > 0)
                {
                    _pinCode = _pinCode.Substring(0, _pinCode.Length - 1);
                    IPDisplay.text = _pinCode;
                }
                break;
            default:
                break;
        }

        if(_pinCode == _code)
        {
            OnAccessGranted.Invoke();
        }
    }
}
