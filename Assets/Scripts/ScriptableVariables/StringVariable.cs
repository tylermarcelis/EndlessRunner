using UnityEngine;

[CreateAssetMenu(fileName = "string_", menuName = "Variables/StringVariable")]
public class StringVariable : GenericVariable<string> {}

[System.Serializable]
public class StringReference : GenericReference<string, StringVariable> { }
