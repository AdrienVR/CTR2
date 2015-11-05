using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EventTrigger))]
public class MenuButton : NavigationCross
{
    public Menu ParentMenu;
    public CrossMenuButton Cross;
    public Menu NextMenu;

    public Animator Animator;

    public bool Active
    {
        get
        {
            return ParentMenu.SelectedButton == this;
        }
        set
        {
            if (value == true)
            {
                ParentMenu.SelectedButton = this;
                ButtonSelect();
            }
            else
                Deselect();
        }
    }

    void Awake()
    {
        m_callback = GetComponent<Button>().onClick;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (Active)
        {
            if (IsHighLighted() == false)
                Animator.Play("Highlighted");

            m_timer += Time.deltaTime;
            if (m_timer < 0.05f)
                return;

            base.Update();
        }
        else
        {
            if (IsHighLighted())
                Deselect();
            m_timer = 0;
        }
    }

    public override void OnLeft()
    {
        if (Cross.Left != null)
        {
            Cross.Left.ReceiveLeft();
        }
    }
    public override void OnRight()
    {
        if (Cross.Right != null)
        {
            Cross.Right.ReceiveRight();
        }
    }
    public override void OnUp()
    {
        if (Cross.Up != null && UpAction())
        {
            Cross.Up.ReceiveUp();
        }
    }
    public override void OnDown()
    {
        if (Cross.Down != null)
        {
            Cross.Down.ReceiveDown();
        }
    }

    public void ReceiveLeft()
    {
        Cross.Right.Active = false;
        Active = true;
    }
    public void ReceiveRight()
    {
        Cross.Left.Active = false;
        Active = true;
    }
    public void ReceiveUp()
    {
        Cross.Down.Active = false;
        Active = true;
    }
    public void ReceiveDown()
    {
        Cross.Up.Active = false;
        Active = true;
    }

    public override void OnValidateAction()
    {
        if (NextMenu == Menu.CurrentMenu.LastMenu)
        {
            if (Menu.CurrentMenu.LastMenu == null)
            {
                AudioManager.Instance.Play("errorMenu");
            }
            else
            {
                AudioManager.Instance.Play("cancelMenu");
                Menu.CurrentMenu.MenuAction.OnHideBack();
            }
        }
        else
        {
            AudioManager.Instance.Play("validateMenu");
            if (NextMenu != null)
            {
                Menu.CurrentMenu.MenuAction.OnHideNext(NextMenu);
            }
        }
        if (m_callback != null)
        {
            m_callback.Invoke();
        }
    }

    public void Select()
    {
        Animator.Play("Highlighted");
    }

    private void Deselect()
    {
        Animator.Play("Normal");
    }

    public void ButtonSelect()
    {
        AudioManager.Instance.Play("downMenu");
        Select();
    }

    public void MouseSelect()
    {
        ParentMenu.SelectedButton = this;
        ButtonSelect();
    }

    public void MouseClick()
    {
        OnValidateAction();
    }

    private bool IsHighLighted()
    {
        return Animator.GetCurrentAnimatorStateInfo(0).IsName("Highlighted");
    }


    private UnityEvent m_callback;

    private bool m_selected;
}
