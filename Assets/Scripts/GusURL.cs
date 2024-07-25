using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GusURL : MonoBehaviour
{
    public void OpenURL()
    {
        Application.OpenURL("https://www.linkedin.com/in/ttavojimenez/");
        Debug.Log("is this working");
    }    
}
