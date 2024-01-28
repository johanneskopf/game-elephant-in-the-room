using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class TextDisplayer : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI textPro;

    private List<TextToDisplay> _textsToDisplay = new();
    private bool _currentlyDisplaying = false;
    private float _waitTime = 0;
    
    void Start()
    {
        panel.SetActive(false);
    }

    void Update()
    {
        if(!_currentlyDisplaying && _textsToDisplay.Count > 0)
        {
            _currentlyDisplaying = true;
            var textToDisplay = _textsToDisplay[0];
            
            _waitTime = textToDisplay.time;
            textPro.text = textToDisplay.text;
            panel.SetActive(true);
            AudioManager.Instance.PlayTextSound();
            
            _textsToDisplay.RemoveAt(0);
        }else if (_waitTime <= 0)
        {
            _currentlyDisplaying = false;
            panel.SetActive(false);
            textPro.text = "nothing to see here";
        }
        
        _waitTime -= Time.deltaTime;
    }

    public void DisplayText(string text, float time)
    {
        Debug.Log("Displaying text: " + text+ " for " + time + " seconds");
        _textsToDisplay.Add(new TextToDisplay()
        {
            text = text,
            time = time
        });
    }
}