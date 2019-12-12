using UnityEngine;

[CreateAssetMenu(fileName = "transform_", menuName = "Variables/TransformVariable")]
public class TransformVariable : GenericVariable<Transform> {}

[System.Serializable]
public class TransformReference : GenericReference<Transform, TransformVariable> {}
