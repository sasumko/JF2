using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIExt
{
    public static T CreateComponent<T> (Transform _parent) where T : Component
    {
        GameObject _go = new GameObject();
        T _ret = _go.AddComponent<T>();

        _go.transform.parent = _parent;
        

        return _ret;
    }
}
