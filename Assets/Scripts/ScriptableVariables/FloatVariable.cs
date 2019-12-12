using UnityEngine;

[CreateAssetMenu(fileName = "float_", menuName = "Variables/FloatVariable")]
public class FloatVariable : GenericVariable<float> { }

[System.Serializable]
public class FloatReference : GenericReference<float, FloatVariable> { }