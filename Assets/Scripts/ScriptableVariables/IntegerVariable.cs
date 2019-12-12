using UnityEngine;

[CreateAssetMenu(fileName = "integer_", menuName = "Variables/IntegerVariable")]
public class IntegerVariable : GenericVariable<int>  {}

[System.Serializable]
public class IntReference : GenericReference<int, IntegerVariable> {}
