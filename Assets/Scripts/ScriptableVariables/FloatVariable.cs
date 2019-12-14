using UnityEngine;

[CreateAssetMenu(fileName = "float_", menuName = "Variables/FloatVariable")]
public class FloatVariable : GenericVariable<float>
{
    public void Add(float value)
    {
        Value += value;
    }
}

[System.Serializable]
public class FloatReference : GenericReference<float, FloatVariable>
{
    public void Add(float value)
    {
        Value += value;
    }
}