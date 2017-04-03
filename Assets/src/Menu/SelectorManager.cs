using UnityEngine;
using System;

public class SelectorManager : MenuButton
{
    public bool HorizontalLoop;
    public bool VerticalLoop;
    [Serializable]
    public class LineSelectorParent
    {
        public Transform[] Arrays;
    }

    public LineSelectorParent[] Lines;

    public override void UpdateInput(MenuInput _input)
    {
    }

    public void MoveSelector(NavigationSelector _selector, MenuInput _direction)
    {
        if (_selector.PlayerIndex == 0 && !m_active)
            return;

        bool result = false;
        if (! _selector.Locked)
        {
            switch (_direction)
            {
                case MenuInput.Up:
                    result = MoveToUp(_selector);
                    break;
                case MenuInput.Down:
                    result = MoveToDown(_selector);
                    break;
                case MenuInput.Left:
                    result = MoveToLeft(_selector);
                    break;
                case MenuInput.Right:
                    result = MoveToRight(_selector);
                    break;
                default:
                    break;
            }
        }
        if (!result)
        {
            Move(_direction);
        }
    }

    public bool MoveToRight(NavigationSelector _selector)
    {
        if (_selector.PlayerIndex == 0 && !HorizontalLoop && _selector.X + 1 >= Lines[_selector.Y].Arrays.Length)
            return false;
        _selector.X = Mod((_selector.X + 1), Lines[_selector.Y].Arrays.Length);
        ApplySelection(_selector);
        return true;
    }

    public bool MoveToLeft(NavigationSelector _selector)
    {
        if (_selector.PlayerIndex == 0 && !HorizontalLoop && _selector.X - 1 < 0)
            return false;
        _selector.X = Mod((_selector.X - 1), Lines[_selector.Y].Arrays.Length);
        ApplySelection(_selector);
        return true;
    }

    public bool MoveToUp(NavigationSelector _selector)
    {
        if (_selector.PlayerIndex == 0 && !VerticalLoop && _selector.Y - 1 < 0)
            return false;
        _selector.Y = Mod((_selector.Y - 1), Lines.Length);
        ApplySelection(_selector);
        return true;
    }

    public bool MoveToDown(NavigationSelector _selector)
    {
        if (_selector.PlayerIndex == 0 && !VerticalLoop && _selector.Y + 1 >= Lines.Length)
            return false;
        _selector.Y = Mod((_selector.Y + 1), Lines.Length);
        ApplySelection(_selector);
        return true;
    }

    private void ApplySelection(NavigationSelector _selector)
    {
        var newSelection = Lines[_selector.Y].Arrays[_selector.X];
        _selector.CurrentSelection = newSelection.name;
        _selector.transform.position = newSelection.position;
    }

    private int Mod(int x, int m)
    {
        return (x % m + m) % m;
    }
}
