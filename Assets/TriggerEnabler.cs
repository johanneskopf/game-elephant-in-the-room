using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnabler : MonoBehaviour
{
    public Enabler enabler;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        enabler.Enable();
    }
}
