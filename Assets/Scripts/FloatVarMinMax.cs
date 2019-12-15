using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatVarMinMax : MonoBehaviour
{
    public FloatReference output;

    public enum MinMaxType
    {
        Minimum,
        Maximum
    }

    public MinMaxType type;

    [SerializeField]
    protected FloatVariable[] variables;

    private void OnEnable()
    {
        foreach (FloatVariable v in variables)
        {
            v.OnChangeValue += CalculateOutput;
        }
    }

    private void CalculateOutput(float value)
    {
        if (variables.Length < 1)
            return;

        float v = variables[0].Value;

        for (int i = 1; i < variables.Length; i++)
        {
            v = Compare(v, variables[i].Value, type);
        }

        if (output.Value != v)
            output.Value = v;
    }

    private void OnDisable()
    {
        foreach (FloatVariable v in variables)
        {
            v.OnChangeValue -= CalculateOutput;
        }
    }

    protected float Compare(float a, float b, MinMaxType compareType)
    {
        if (compareType == MinMaxType.Maximum)
            return Mathf.Max(a, b);

        return Mathf.Min(a, b);
    }
}
