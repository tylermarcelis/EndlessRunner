using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class FloatUI : MonoBehaviour
{
    public FloatReference displayValue;
    public bool roundToInt;
    Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void OnEnable()
    {
        if (displayValue.variable)
            displayValue.variable.OnChangeValue += OnValueChange;

        OnValueChange(displayValue);
    }


    private void OnDisable()
    {
        if (displayValue.variable)
            displayValue.variable.OnChangeValue -= OnValueChange;
    }

    private void OnValueChange(float value)
    {
        string displayText = (roundToInt) ? Mathf.Round(displayValue).ToString() : displayValue.ToString();
        text.text = displayText;
    }
}
