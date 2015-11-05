
using UnityEngine;

public class NavigationCross : MonoBehaviour
{
    protected virtual void Update()
    {
        m_timer += Time.deltaTime;
        if (m_timer < 0.05f)
            return;

        if (ValidateAction())
        {
            OnValidateAction();
        }
        if (LeftAction())
        {
            OnLeft();
        }
        if (RightAction())
        {
            OnRight();
        }
        if (UpAction())
        {
            OnUp();
        }
        if (DownAction())
        {
            OnDown();
        }
    }

    public virtual void OnLeft() { }
    public virtual void OnRight() { }
    public virtual void OnUp() { }
    public virtual void OnDown() { }
    public virtual void OnValidateAction() { }

    protected virtual bool UpAction()
    {
        return ControllerManager.Instance.GetKeyDown("up") || Input.GetKeyDown(KeyCode.UpArrow);
    }

    protected virtual bool DownAction()
    {
        return ControllerManager.Instance.GetKeyDown("down") || Input.GetKeyDown(KeyCode.DownArrow);
    }

    protected virtual bool LeftAction()
    {
        return ControllerManager.Instance.GetKeyDown("left") || Input.GetKeyDown(KeyCode.LeftArrow);
    }

    protected virtual bool RightAction()
    {
        return ControllerManager.Instance.GetKeyDown("right") || Input.GetKeyDown(KeyCode.RightArrow);
    }

    protected virtual bool ValidateAction()
    {
        return ControllerManager.Instance.GetKeyDown("validate") || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter);
    }

    protected float m_timer;
}
