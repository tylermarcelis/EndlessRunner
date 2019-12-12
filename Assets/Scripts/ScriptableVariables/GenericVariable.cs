using UnityEngine;

public abstract class GenericVariable<T> : ScriptableObject
{
    [SerializeField] protected T value;
    public T Value
    {
        get
        {
            return value;
        }
        set
        {
            this.value = value;
            OnChangeValue?.Invoke(value);
        }
    }

    public delegate void ChangeValue(T value);
    public event ChangeValue OnChangeValue;
}

public abstract class GenericReference<T, U> where U : GenericVariable<T>
{
    public bool useConstant = true;

    public T constantVariable;

    public U variable;

    // property for setting Value on either the constant or the ScriptableVariable
    public T Value
    {
        get { if (!useConstant && variable == null) { Debug.LogWarning("A reference is not using constant but no variable is set!"); } return useConstant || variable == null ? constantVariable : variable.Value; }
        set { if (useConstant == true) { constantVariable = value; } else { variable.Value = value; } }
    }

    //what if this was more transparent
    public static implicit operator T(GenericReference<T,U> i)
    {
        return i.Value;
    }

    //make a deep copy
    public static V Copy<V>(V original) where V : GenericReference<T,U>, new()
    {
        V copy = new V
        {
            useConstant = original.useConstant,
            constantVariable = original.constantVariable
        };

        if (original.variable != null)
        {
            copy.variable = ScriptableObject.CreateInstance<U>();
            copy.variable.Value = original.variable.Value;
        }
        return copy;
    }

    public V Copy<V>() where V : GenericReference<T, U>, new()
    {
        return Copy((V)this);
    }

    public void CopyFrom<V>(V other) where V : GenericReference<T, U>, new()
    {
        useConstant = other.useConstant;
        constantVariable = other.constantVariable;

        if (other.variable != null)
        {
            variable = ScriptableObject.CreateInstance<U>();
            variable.Value = other.variable.Value;
        }
    }

    public void CopyTo<V>(V other) where V : GenericReference<T, U>, new()
    {
        other.useConstant = useConstant;
        other.constantVariable = constantVariable;

        if (variable != null)
        {
            other.variable = ScriptableObject.CreateInstance<U>();
            other.variable.Value = variable.Value;
        }
    }
}
