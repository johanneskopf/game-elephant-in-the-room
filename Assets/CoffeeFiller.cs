using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeFiller : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var coffeeCup = other.GetComponent<CoffeeCup>();
        if (coffeeCup != null)
        {
            coffeeCup.Fill();
        }
    }
}
