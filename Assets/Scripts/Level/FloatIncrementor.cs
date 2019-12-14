using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatIncrementor : MonoBehaviour
{
    public BoolReference doIncrement;
    public FloatReference incrementedValue;
    public FloatReference incrementAmount;

    public bool limit;
    public float maximum;

    private void Update()
    {
        // If should increment and is under its limit
        if (doIncrement && (!limit || incrementedValue < maximum))
        {
            // If limited, clamps newValue to be less than maximum, otherwise just adds
            float newValue = (limit) ?
                Mathf.Min(incrementedValue.Value + incrementAmount * Time.deltaTime, maximum) :
                incrementedValue.Value + incrementAmount * Time.deltaTime;

            incrementedValue.Value = newValue;
        }
    }
}
