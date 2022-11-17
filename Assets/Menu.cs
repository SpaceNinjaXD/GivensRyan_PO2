using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public void OnSTART()
    {
        Application.LoadLevel(1);
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
