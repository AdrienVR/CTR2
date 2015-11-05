using UnityEngine;
using System;

public class SelectorManager : MonoBehaviour
{
    [Serializable]
    public class LineSelectorParent
    {
        public Transform[] Arrays;
    }

    public LineSelectorParent[] Lines;

    public Transform GetRightParent(ref int x, ref int y)
    {
        x = Mod((x + 1), Lines[y].Arrays.Length);
        return Lines[y].Arrays[x];
    }

    public Transform GetLeftParent(ref int x, ref int y)
    {
        x = Mod((x - 1), Lines[y].Arrays.Length);
        return Lines[y].Arrays[x];
    }

    public Transform GetUpParent(ref int x, ref int y)
    {
        y = Mod((y - 1), Lines.Length);
        return Lines[y].Arrays[x];
    }

    public Transform GetDownParent(ref int x, ref int y)
    {
        y = Mod((y + 1), Lines.Length);
        return Lines[y].Arrays[x];
    }

    private int Mod(int x, int m)
    {
        return (x % m + m) % m;
    }
}
