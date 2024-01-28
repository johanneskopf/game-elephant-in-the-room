using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTextTrigger : MonoBehaviour
{
    private TextDisplayer _textDisplayer;

    public string[] text;
    public float time = 5;

    void Start()
    {
        _textDisplayer = FindFirstObjectByType<TextDisplayer>();
        if (_textDisplayer == null)
        {
            Debug.LogError("No TextDisplayer found in scene.");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach (var t in text)
            {
                _textDisplayer.DisplayText(t, time);
            }

            Destroy(gameObject);
        }
    }
}