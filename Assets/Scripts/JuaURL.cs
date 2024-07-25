using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuaURL : MonoBehaviour
{
    public void OpenURL()
    {
        Application.OpenURL("https://www.linkedin.com/in/changuaconqueso/");
        Debug.Log("is this working");
    }    
}
