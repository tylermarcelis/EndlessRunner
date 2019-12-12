using UnityEngine;

[CreateAssetMenu(fileName = "bool_", menuName = "Variables/BoolVariable")]
public class BoolVariable : GenericVariable<bool> {}

[System.Serializable]
public class BoolReference : GenericReference<bool, BoolVariable> { }
