using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeCup : MonoBehaviour
{
    public bool filled = false;

    public Mesh filledCoffeeMesh;

    public void Fill()
    {
        if(filled) return;
        
        Debug.Log("Filling coffee cup");
        filled = true;
        GetComponent<MeshFilter>().mesh = filledCoffeeMesh;
    }
}
