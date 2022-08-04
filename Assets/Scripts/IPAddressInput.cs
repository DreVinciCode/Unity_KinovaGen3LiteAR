using System;
using System.Linq;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IPAddressInput : MonoBehaviour
{
    public TMP_Text IPDisplay;
    public GameObject ContinueButton;

    private string _uniqueIP;
    private string _iPAddress = "192.168.0.23";

    private void Start()
    {
        _uniqueIP = gameObject.transform.parent.transform.parent.name;
        _iPAddress = PlayerPrefs.GetString(_uniqueIP);
        ContinueButton.SetActive(ValidateIPv4(_iPAddress));
        IPDisplay.text = _iPAddress;
    }

    public void AddDigitSequence()
    {
        AR_PushButton.ARButtonPressed += AddDigitToIPSequence;
    }

    public void RemoveDigitSequence()
    {
        AR_PushButton.ARButtonPressed -= AddDigitToIPSequence;
    }

    public bool ValidateIPv4(string ipString)
    {
        if (String.IsNullOrEmpty(ipString))
        {
            return false;
        }

        string[] splitValues = ipString.Split('.');
        if (splitValues.Length != 4)
        {
            return false;
        }

        byte tempForParsing;

        return splitValues.All(r => byte.TryParse(r, out tempForParsing));
    }

    private void AddDigitToIPSequence(string digitsEntered)
    {
        if (_iPAddress.Length > 15)
        {
            _iPAddress = _iPAddress.Substring(0, _iPAddress.Length - 1);
            PlayerPrefs.SetString(_uniqueIP, _iPAddress);
        }

        switch (digitsEntered)
        {
            case "0":
                _iPAddress += "0";
                IPDisplay.text = _iPAddress;
                break;

            case "1":
                _iPAddress += "1";
                IPDisplay.text = _iPAddress;
                break;
            case "2":
                _iPAddress += "2";
                IPDisplay.text = _iPAddress;
                break;
            case "3":
                _iPAddress += "3";
                IPDisplay.text = _iPAddress;
                break;
            case "4":
                _iPAddress += "4";
                IPDisplay.text = _iPAddress;
                break;
            case "5":
                _iPAddress += "5";
                IPDisplay.text = _iPAddress;
                break;
            case "6":
                _iPAddress += "6";
                IPDisplay.text = _iPAddress;
                break;
            case "7":
                _iPAddress += "7";
                IPDisplay.text = _iPAddress;
                break;
            case "8":
                _iPAddress += "8";
                IPDisplay.text = _iPAddress;
                break;
            case "9":
                _iPAddress += "9";
                IPDisplay.text = _iPAddress;
                break;
            case ".":
                _iPAddress += ".";
                IPDisplay.text = _iPAddress;
                break;
            case "x":
                if (_iPAddress.Length > 0)
                {
                    _iPAddress = _iPAddress.Substring(0, _iPAddress.Length - 1);
                    IPDisplay.text = _iPAddress;
                }
                break;
            default:
                break;       
        }

        if (ValidateIPv4(_iPAddress))
        {
            ContinueButton.SetActive(true);
            PlayerPrefs.SetString(_uniqueIP, _iPAddress);
        }
        else
        {
            ContinueButton.SetActive(false);
        }
    }
}
