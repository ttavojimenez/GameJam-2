using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaiURL  : MonoBehaviour
{
    public void OpenURL()
    {
        Application.OpenURL("https://www.linkedin.com/in/dainercortes/");
        Debug.Log("is this working");
    }    
}