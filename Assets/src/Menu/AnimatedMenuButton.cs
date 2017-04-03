
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class MenuButton : MonoBehaviour
{
    public MenuButtonCross Cross;
    public Menu NextMenu;
    protected bool m_active;

    public virtual void OnActivate()
    {
        if (MainMenuManager.Instance.CurrentButton != this)
            MainMenuManager.Instance.CurrentButton = this;
        m_active = true;
    }
    public virtual void OnDeactivate() { m_active = false; }
    public virtual void OnValidation() { }

    public virtual void UpdateInput(MenuInput _input)
    {
        if (_input == MenuInput.Validation)
            OnValidation();
        else
            Move(_input);
    }

    public virtual void Move(MenuInput _input)
    {
        switch (_input)
        {
            case MenuInput.Up:
                if (Cross.Up)
                    MainMenuManager.Instance.CurrentButton = Cross.Up;
                break;
            case MenuInput.Down:
                if (Cross.Down)
                    MainMenuManager.Instance.CurrentButton = Cross.Down;
                break;
            case MenuInput.Left:
                if (Cross.Left)
                    MainMenuManager.Instance.CurrentButton = Cross.Left;
                break;
            case MenuInput.Right:
                if (Cross.Right)
                    MainMenuManager.Instance.CurrentButton = Cross.Right;
                break;
        }
    }
}

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Animator))]
public class AnimatedMenuButton : MenuButton
{
    public EventTrigger.TriggerEvent ValidateCallback;
    
    public Animator Animator;

    public virtual void OnValidate()
    {
        Animator = GetComponent<Animator>();
    }

    public override void OnActivate()
    {
        base.OnActivate();
        Animator.SetBool("Locked", true);
        Animator.Play("Highlighted");
        AudioManager.Instance.Play("downMenu");
    }

    public override void OnDeactivate()
    {
        base.OnDeactivate();
        Animator.SetBool("Locked", false);
        Animator.SetTrigger("Normal");
    }
    
    public override void OnValidation()
    {
        if (ValidateCallback != null)
        {
            ValidateCallback.Invoke(null);
        }
    }

    public void MouseSelect()
    {
        OnActivate();
    }

    public void MouseClick()
    {
        MainMenuManager.Instance.OnMenuInputCB(MenuInput.Validation, 0);
    }

    private bool IsHighLighted()
    {
        return Animator.GetCurrentAnimatorStateInfo(0).IsName("Highlighted");
    }
}
