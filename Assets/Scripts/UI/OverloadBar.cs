using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OverloadBar : VisualElement
{
    private ProgressBar _bar;
    private Label _text;
    public OverloadBar()
    {
        _bar = new ProgressBar();
        AddToClassList("overload-bar-container");
        _bar.AddToClassList("overload-bar");
        _bar.name = "bar";
        RelabelChildren(_bar, 0);

        _bar.Q<VisualElement>("10").AddToClassList("overload-bar-inner-style-1");
        _bar.Q<VisualElement>("20").AddToClassList("overload-bar-inner-style-2");
        _text = _bar.Q<Label>();
        _text.text = "OVERLOAD";
        _text.AddToClassList("overload-bar-label");
        Add(_bar);

        _bar.lowValue = 0;
        _bar.highValue = 1;
        EmptyProgressBar();

    }


    public void EmptyProgressBar()
    {
        _bar.value = _bar.lowValue;
    }

    internal void SetBackgroundToRed()
    {
        _bar.Q<VisualElement>("20").style.backgroundColor = Color.red;
    }

    public void SetFontColor(Color c)
    {
        _text.style.color = c;
    }

    public void SetFontSize(int sizeInPx)
    {
        _text.style.fontSize = sizeInPx;
    }

    public void SetFontBold()
    {
        _text.AddToClassList("text-bold");
    }


    public void FillProgressBar()
    {
        _bar.value = _bar.highValue;
    }

    public void FillToMax()
    {

    }

    public void SetValue(float value)
    {
        _bar.value = value;
    }

    public float GetValue() => _bar.value;


    private void RelabelChildren(VisualElement ve, int prefix)
    {
        int indexer = 0;
        foreach (var child in ve.Children())
        {
            child.name = $"{prefix}{(indexer++)}";
            RelabelChildren(child, prefix + 1);
        }
    }
    public new class UxmlFactory : UxmlFactory<OverloadBar, UxmlTraits> { }

    public new class UxmlTraits : VisualElement.UxmlTraits { }
}
