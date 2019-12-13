using UnityEngine;

[CreateAssetMenu(fileName = "vector2_", menuName = "Variables/Vector2Variable")]
public class Vector2Variable : GenericVariable<Vector2> { }

[System.Serializable]
public class Vector2Reference : GenericReference<Vector2, Vector2Variable> { }
