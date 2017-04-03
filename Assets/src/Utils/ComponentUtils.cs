
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

public static class ComponentUtils
{
    public static T GetByNameInChildren<T>(GameObject _obj, string _name) where T : Component, IEnumerable
    {
        List<T> list = new List<T>(_obj.GetComponentsInChildren<T>());
        T temp = list.Where(obj => obj.name == _name).SingleOrDefault();
        return temp;
    }
}
