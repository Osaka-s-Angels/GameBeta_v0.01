using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public GameObject GameObject;
    
    void Start()
    {
        GameObject.SetActive(true);
    }


}
