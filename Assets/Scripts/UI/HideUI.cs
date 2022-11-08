using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideUI : MonoBehaviour
{
    public GameObject canvas;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Comma)) {
            if (canvas.activeInHierarchy)
                canvas.SetActive(false);
            else 
                canvas.SetActive(true);
        }        
    }
}
