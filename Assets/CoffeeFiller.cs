using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeFiller : MonoBehaviour
{
    private float steamLeft = 0;

    public ParticleSystem particleSystem;

    void Start()
    {
        particleSystem.Stop();
    }

    private void Update()
    {
        if (steamLeft > 0)
        {
            steamLeft -= Time.deltaTime;
            if (steamLeft <= 0)
            {
                particleSystem.Stop();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var coffeeCup = other.GetComponent<CoffeeCup>();
        if (coffeeCup != null)
        {
            coffeeCup.Fill();
            steamLeft = 0.5f;
            particleSystem.Play();
        }
    }
}