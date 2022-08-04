using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManuBehavior : MonoBehaviour
{
    public GameObject MainHandMenu;
    public GameObject UR5HandMenu;
    public GameObject AR_NumMenu;


    public void SummonMainMenu()
    {
        if (!UR5HandMenu.activeSelf && !AR_NumMenu.activeSelf)
            MainHandMenu.SetActive(true);
    }

    public void DismissMainMenu()
    {
        MainHandMenu.SetActive(false);
    }

}
